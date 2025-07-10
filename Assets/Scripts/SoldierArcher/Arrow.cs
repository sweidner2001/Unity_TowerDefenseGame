using System;
using System.Collections;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Arrow : MonoBehaviour
{
    //######################## Membervariablen ##############################
    private Rigidbody2D rb;

    // Welche GameObjekts kann ich treffen? (müssen Colllider besitzen)
    protected LayerMask enemyLayer;
    public LayerMask obstacleLayer;

    // Sprite wechseln, wenn Gegner getroffen:
    protected SpriteRenderer sr;
    public Sprite objectHitSprite;

    // Pfeil Konfiguration:
    protected Transform enemyTransform;
    protected ConfigArrow Config { get; set; }
    public event Action<Collision2D> OnEnemyArrowCollision;



    public void Init(ConfigArrow config, Transform enemyTransform, Action<Collision2D> handleArrowCollisionWithEnemy, LayerMask enemyLayer)
    {
        this.Config = config;
        this.enemyTransform = enemyTransform;
        this.OnEnemyArrowCollision += handleArrowCollisionWithEnemy;
        this.enemyLayer = enemyLayer;
    }


    //########################### Geerbte Methoden #############################
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();   
        this.sr = GetComponent<SpriteRenderer>();


        rb.gravityScale = 0;
        RotateArrowBeforeAttack();

        if (this.enemyTransform != null)
        {
            StartCoroutine(FlyAlongBezierDynamic());
        }
    }


    void Update()
    {
        
    }



    //########################### Methoden #############################
    public static float GetBowRotationAngle(Vector2 startPoint, Vector2 endPoint, ConfigArrow config)
    {

        float normDist = GetNormFromDist(startPoint, endPoint);
        Vector2 P1 = GetP1(startPoint, endPoint, config, normDist);
        return GetArrowRotationAngle(P0: startPoint, P1: P1, P2: endPoint, t: 0);
    }

    private void RotateArrowBeforeAttack()
    {
        float angle = Arrow.GetBowRotationAngle(this.transform.position, this.enemyTransform.position, this.Config);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }



    //~~~~~~~~~~~~~~~~~~~~ Flugbahn Berechnung ~~~~~~~~~~~~~~~~~~~~~~
    private static float GetNormFromDist(Vector2 P0, Vector2 P2)
    {
        float distanceToEnemy = Vector2.Distance(P0, P2);
        float normDist = Mathf.Clamp01(distanceToEnemy / 10);
        return normDist;
    }

    private static Vector2 GetP1(Vector2 P0, Vector2 P2, ConfigArrow config, float normDist)
    {
        float arcHeight = Mathf.Max(0, Mathf.Lerp(config.minArcHeight, config.maxArcHeight, normDist));
        Vector2 midPoint = (P0 + P2) * 0.5f;
        Vector2 P1 = midPoint + Vector2.up * arcHeight;
        return P1;
    }


    private static float GetFlightDuration(ConfigArrow config, float normDist)
    {
        return Mathf.Max(0, Mathf.Lerp(config.minFlightDuration, config.maxFlightDuration, normDist));
    }



    private static float GetArrowRotationAngle(Vector2 P0, Vector2 P1, Vector2 P2, float t)
    {
        Vector2 tangent =
            2 * (1 - t) * (P1 - P0)
            + 2 * t * (P2 - P1);
        float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
        return angle;
    }




    //####################### Coroutinen ###########################
    private IEnumerator FlyAlongBezierDynamic()
    {
        float currentFlightTime = 0f;

        // 1) P0: Startpunkt; P1 = Mittelpunkt Kurve; P2: Endpunkt
        Vector2 P0 = this.transform.position;
        Vector2 P2 = this.enemyTransform.position;
        Vector2 P2atStart = this.enemyTransform.position;
        float normDist = GetNormFromDist(P0, P2);
        float flightDuration = GetFlightDuration(this.Config, normDist);
        Vector2 P1 = GetP1(P0, P2, Config, normDist);

        
        while (currentFlightTime < flightDuration)
        {
            if (enemyTransform == null)
            {
                // Gegner Existiert nicht mehr, wurde aber auch nicht getroffen --> Pfeil landet in Tilemap
                if (sr.sprite != objectHitSprite)
                    AttachToTilemap();
                yield break;
            }

            // Gegner kann Pfeil entwischen, wenn er sich zu weit bewegt hat
            float distanceToP2atStart = Vector2.Distance(P2atStart, this.enemyTransform.position);
            if (distanceToP2atStart < this.Config.maxDistanceFromStartPosToUpdateEnemyPos)
            {
                // Pfeil soll den Gegner verfolgen:
                P2 = this.enemyTransform.position;
            }

            // Parameter t: 0 -> 1
            float t = currentFlightTime / flightDuration;

            // Quadratische Bezier-Gleichung
            Vector2 pos = Mathf.Pow(1 - t, 2) * P0
                        + 2 * (1 - t) * t * P1
                        + t * t * P2;

            // Setze Position des Pfeils
            Vector2 delta = pos - rb.position;
            Vector2 neededVel = delta / Time.fixedDeltaTime;
            rb.linearVelocity = neededVel;

            // Rotation des Pfeis in Flugrichtung
            float angle = GetArrowRotationAngle(P0, P1, P2, t);
            transform.rotation = Quaternion.Euler(0, 0, angle);

            currentFlightTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Zeit ist abgelaufen und Pfeil hat keinen Gegner getroffen
        AttachToTilemap();

    }





    //~~~~~~~~~~~~~~~~~~~~~~ Gegner getroffen ~~~~~~~~~~~~~~~~~~~~~~
    public void OnCollisionEnter2D(Collision2D collision)
    {

        // Der Ausdruck (1 << collision.gameObject.layer) verschiebt das Bit 1 um so viele Stellen nach links,
        // wie es die Layer-ID des kollidierenden Objekts angibt. Funktioniert nur, weil die Layer intern als
        // 2er Potenzen dargestellt werden
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            OnEnemyArrowCollision?.Invoke(collision);
            AttachToTarget(collision.collider.transform);
            Destroy(gameObject, this.Config.lifeSpanOnHittedObject);
        }
        else if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Pfeil soll auch an anderen Objekten hängen bleiben
            AttachToTarget(collision.gameObject.transform);
            Destroy(gameObject, this.Config.lifeSpanOnHittedObject);
        }
    }



    private void AttachToTarget(Transform target=null)
    {
        // Pfeil ist am Ziel angekommen
        this.enemyTransform = null;

        // 1. Bild austauschen:
        sr.sprite = objectHitSprite;

        // 2. Pfeil stopen:
        rb.linearVelocity = Vector2.zero;

        // 3. Physik abschalten: Rigidbody auf Kinematic setzen
        rb.bodyType = RigidbodyType2D.Kinematic;

        // 4. Collider deaktivieren, damit der Pfeil keine Kollisionen/Schaden mehr verursacht
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // 5. Pfeil an Ziel binden
        if (target != null)
            transform.SetParent(target);
    }

    private void AttachToTilemap()
    {
        AttachToTarget(null);
        Destroy(gameObject, this.Config.lifeSpanOnHittedTilemap);
    }



}
