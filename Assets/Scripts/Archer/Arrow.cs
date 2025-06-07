using System;
using System.Collections;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Arrow : MonoBehaviour
{


    //######################## Membervariablen ##############################
    private Rigidbody2D rb;

    // Eigenschaften des Pfeils
    public Vector2 ArrowDirection { set; get; } = Vector2.right;
    //public float maxLifeSpanOnFlying = 3;                      // Max. Lebenszeit in s des Pfeils
    //public float LifeSpanOnHittedObject { get; set; } = 2;                      // Max. Lebenszeit in s des Pfeils
    //public float LifeSpanOnHittedTilemap { get; set; } = 1;

    // Welche GameObjekts kann ich treffen? (müssen Colllider besitzen)
    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;

    // Sprite wenn Gegner getroffen:
    private SpriteRenderer sr;
    public Sprite objectHitSprite;

    public float ArrowSpeed { get; set; } = 6;                // Geschwindigkeit des Pfeils
    public Transform enemyTransform;
    //public Transform destTransformStatic;
    public ArrowConfig Config { get; set; }

    // Action-Delegate statt eigener Delegate-Definition
    public event Action<Collision2D> OnEnemyArrowCollision;


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();   
        this.sr = GetComponent<SpriteRenderer>();


        rb.gravityScale = 0;

        if (this.enemyTransform != null)
        {
            StartCoroutine(FlyAlongBezierDynamic());
        }
        //rb.linearVelocity = this.ArrowDirection * this.ArrowSpeed;
        //RotateArrowBeforeAttack();
    }



    // Update is called once per frames
    void Update()
    {
        
    }
    private float timer = 3f;

    private void FixedUpdate()
    {
        // 1) Pfeil folgt dem Gegner, wenn er vorhanden ist
        //timer -= Time.deltaTime;

        //if(timer <= 0 && enemyTransform != null)
        //{
        //    // An Tilemap anhängen:
        //    AttachToTarget();
        //    Destroy(gameObject, 2);
        //}

        //if (enemyTransform != null)
        //{
        //    FollowEnemy();
        //    Destroy(gameObject, maxLifeSpanOnFlying);
        //}
    }


    //########################### Methoden #############################

    //public void ShootArrow(Vector2 arrowDirection, float arrowSpeed)
    //{
    //    if (this.rb == null)
    //    {
    //        Start();
    //    }

    //    this.ArrowDirection = arrowDirection;
    //    rb.linearVelocity = this.ArrowDirection * arrowSpeed;
    //    RatateArrow();
    //    Destroy(gameObject, maxLifeSpan);
    //}


    // Für Schusswaffen
    //private void FollowEnemy()
    //{
    //    // 1) Flugrichtung zum Gegner anpassen
    //    Vector2 direction = (enemyTransform.position - transform.position).normalized;
    //    rb.linearVelocity = direction * this.ArrowSpeed;

    //    // 2) Rotation an Flugrichtung anpassen
    //    float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
    //    transform.rotation = Quaternion.Euler(0, 0, angle);
    //}


   

    //[Header("Kurven-Parameter")]
    //public float maxArcHeight = 2f;                            // Wie hoch der Bogen sein soll
    //public float maxFlightDuration = 2.0f;                     // Zeit in Sekunden, bis der Pfeil ankommt
    //private float minArcHeight = -0.5f;                            // Wie hoch der Bogen sein soll
    //private float minFlightDuration = 0;                     // Zeit in Sekunden, bis der Pfeil ankommt
    //public float maxDistanceFromStartPosToUpdateEnemyPos = 2;
    public float MaxFlightDistance { get; set; } = 7;



    private static void GetBezierParameters(Vector2 startPoint, Vector2 endPoint, ArrowConfig config, float maxFlyingDistance, out Vector2 P1, out float flightDuration)
    {
        float normDist = GetNormFromDist(startPoint, endPoint, maxFlyingDistance);
        flightDuration = Mathf.Max(0, Mathf.Lerp(config.minFlightDuration, config.maxFlightDuration, normDist));

        float arcHeight = Mathf.Max(0, Mathf.Lerp(config.minArcHeight, config.maxArcHeight, normDist));
        Vector2 midPoint = (startPoint + endPoint) * 0.5f;
        P1 = midPoint + Vector2.up * arcHeight;
    }

    private static Vector2 GetP1(Vector2 P0, Vector2 P2, ArrowConfig config, float normDist)
    {
        float arcHeight = Mathf.Max(0, Mathf.Lerp(config.minArcHeight, config.maxArcHeight, normDist));
        Vector2 midPoint = (P0 + P2) * 0.5f;
        Vector2 P1 = midPoint + Vector2.up * arcHeight;
        return P1;
    }


    private static float GetFlightDuration(ArrowConfig config, float normDist)
    {
        return Mathf.Max(0, Mathf.Lerp(config.minFlightDuration, config.maxFlightDuration, normDist));
    }


    IEnumerator FlyAlongBezierDynamic()
    {
        float currentFlightTime = 0f;

        // 1) P0: Startpunkt; P1 = Mittelpunkt Kurve; P2: Endpunkt
        Vector2 P0 = this.transform.position;
        Vector2 P2 = this.enemyTransform.position;
        Vector2 P2atStart = this.enemyTransform.position;
        float normDist = GetNormFromDist(P0, P2, this.MaxFlightDistance);
        float flightDuration = GetFlightDuration(this.Config, normDist);
        Vector2 P1 = GetP1(P0, P2, Config, normDist);

        GetBezierParameters(P0, P2, this.Config, this.MaxFlightDistance, out P1, out flightDuration);

        while (currentFlightTime < this.Config.maxFlightDuration)
        {
            if (enemyTransform == null)
            {
                //AttachToTilemap();
                //Destroy(gameObject, LifeSpanOnHittedTilemap);
                yield break; // Beende die Coroutine, wenn kein Ziel mehr vorhanden ist
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

    private static float GetArrowRotationAngle(Vector2 P0, Vector2 P1, Vector2 P2, float t)
    {
        Vector2 tangent =
            2 * (1 - t) * (P1 - P0)
            + 2 * t * (P2 - P1);
        float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
        return angle;
    }

    public static float GetBowRotationAngle(Vector2 startPoint, Vector2 endPoint, ArrowConfig config, float maxFlightDistance)
    {

        float normDist = GetNormFromDist(startPoint, endPoint, maxFlightDistance);
        Vector2 P1 = GetP1(startPoint, endPoint, config, normDist);
        return GetArrowRotationAngle(P0: startPoint, P1: P1, P2: endPoint, t: 0);
    }

    private static float GetNormFromDist(Vector2 P0, Vector2 P2, float maxFlyingDistance)
    {
        float distanceToEnemy = Vector2.Distance(P0, P2);
        float normDist = Mathf.Clamp01(distanceToEnemy / maxFlyingDistance);
        return normDist;
    }

    //IEnumerator FlyAlongBezier2()
    //{
    //    // 1) P0: Startpunkt = aktuelle Position
    //    Vector2 P0 = transform.position;

    //    // 2) P2: Endpunkt = Zielposition (kann hier einfach die aktuelle Position sein;
    //    //    wenn du bewegliche Gegner hast, nimm ggf. ihre prognostizierte Position)
    //    Vector2 P2 = enemyTransform.position;

    //    // 3) P1: Kontrollpunkt = mittlerer Punkt + nach oben verschoben (für Bogen)
    //    Vector2 midPoint = (transform.position + enemyTransform.position) * 0.5f;
    //    Vector2 P1 = midPoint + Vector2.up * arcHeight;

    //    float elapsed = 0f;
    //    while (elapsed < maxFlightDuration)
    //    {
    //        if (enemyTransform == null)
    //        {
    //            //AttachToTilemap();
    //            //Destroy(gameObject, LifeSpanOnHittedTilemap);
    //            yield break; // Beende die Coroutine, wenn kein Ziel mehr vorhanden ist
    //        }

    //        // Parameter t von 0 -> 1
    //        float t = elapsed / maxFlightDuration;

    //        // Quadratische Bezier-Gleichung
    //        Vector2 pos = Mathf.Pow(1 - t, 2) * P0
    //                    + 2 * (1 - t) * t * P1
    //                    + t * t * (Vector2)enemyTransform.position;

    //        // Setze Position des Pfeils
    //        transform.position = pos;

    //        // Optional: Rotation so, dass der Pfeil immer in Flugrichtung zeigt
    //        Vector2 tangent =
    //            2 * (1 - t) * (P1 - P0)
    //            + 2 * t * ((Vector2)enemyTransform.position - P1);
    //        float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
    //        transform.rotation = Quaternion.Euler(0, 0, angle);

    //        elapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //}




    private void RotateArrowBeforeAttack()
    {
        // Winkel zwischen 2 Punkten
        float angle = Mathf.Atan2(ArrowDirection.y, ArrowDirection.x) * Mathf.Rad2Deg;

        // Euler-Winkel als Input und Quaternion Winkel als output (diese sind effizienter)
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {

        // Der Ausdruck (1 << collision.gameObject.layer) verschiebt das Bit 1 um so viele Stellen nach links,
        // wie es die Layer-ID des kollidierenden Objekts angibt. Funktioniert nur, weil die Layer intern als
        // 2er Potenzen dargestellt werden
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            OnEnemyArrowCollision?.Invoke(collision);
            AttachToTarget(collision.gameObject.transform);
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
