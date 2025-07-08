using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public enum DynamiteState
{
    Idle,           // Dynamit ist bereit
    Flying,         // Dynamit fliegt zum Gegner
    Exploding,      // Dynamit explodiert
    Destroyed       // Dynamit wurde zerstört
}

public class Dynamite : MonoBehaviour
{
    //######################## Membervariablen ##############################
    protected Rigidbody2D rb;
    protected Transform dynamiteExplosionPoint;

    // Welche GameObjekts kann ich treffen? (müssen Colllider besitzen)
    protected LayerMask enemyLayer;

    protected DynamiteState state = DynamiteState.Idle;
    private Animator animator;


    // Dynamite Konfiguration:
    protected Transform enemyTransform;
    protected ConfigDynamite Config { get; set; }
    public event Action<Transform, Collider2D> OnDynamiteExplosion;
    public event Action<Transform, Collider2D> OnDynamiteExplosionShockwave;




    public void Init(ConfigDynamite config, Transform enemyTransform, Action<Transform, Collider2D> handleDynamiteExplosion, Action<Transform, Collider2D> handleDynamiteExplosionShockwave, LayerMask enemyLayer)
    {
        this.Config = config;
        this.enemyTransform = enemyTransform;
        this.OnDynamiteExplosion += handleDynamiteExplosion;
        this.OnDynamiteExplosion += handleDynamiteExplosionShockwave;
        this.enemyLayer = enemyLayer;
    }


    //########################### Geerbte Methoden #############################
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.dynamiteExplosionPoint = transform.Find("ColliderForExplosion");


        rb.gravityScale = 0;
        //RotateArrowBeforeAttack();

        if (this.enemyTransform != null)
        {
            state = DynamiteState.Flying;
            StartCoroutine(FlyAlongBezierDynamic());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //############################### Methoden ################################
    protected void DynamiteExplode()
    {
        this.enemyTransform = null;
        state = DynamiteState.Exploding;
        animator.SetTrigger("startExplosion");
    }
    public void DynamiteDestroy()
    {
        state = DynamiteState.Destroyed;
        Destroy(gameObject);
    }



    //private void RotateArrowBeforeAttack()
    //{
    //    float angle = Arrow.GetBowRotationAngle(this.transform.position, this.enemyTransform.position, this.Config);
    //    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    //}



    //~~~~~~~~~~~~~~~~~~~~ Flugbahn Berechnung ~~~~~~~~~~~~~~~~~~~~~~
    private static float GetNormFromDist(Vector2 P0, Vector2 P2)
    {
        float distanceToEnemy = Vector2.Distance(P0, P2);
        float normDist = Mathf.Clamp01(distanceToEnemy / 10);
        return normDist;
    }

    private static Vector2 GetP1(Vector2 P0, Vector2 P2, ConfigDynamite config, float normDist)
    {
        float arcHeight = Mathf.Max(0, Mathf.Lerp(config.minArcHeight, config.maxArcHeight, normDist));
        Vector2 midPoint = (P0 + P2) * 0.5f;
        Vector2 P1 = midPoint + Vector2.up * arcHeight;
        return P1;
    }


    private static float GetFlightDuration(ConfigDynamite config, float normDist)
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
                if(this.state == DynamiteState.Flying)
                {
                    // Gegner existiert nicht mehr, aber Dynamit fliegt noch --> Explosion
                    DynamiteExplode();
                }
                // Gegner Existiert nicht mehr, aber Dynamit schon explodiert
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
        Debug.Log("Arrow: Flight time is over, but no enemy was hit. Arrow will attach to tilemap.");
        DynamiteExplode();

    }





    //~~~~~~~~~~~~~~~~~~~~~~ Gegner getroffen ~~~~~~~~~~~~~~~~~~~~~~
    public void OnTriggerEnter2D(Collider2D collision)
    {

        // Der Ausdruck (1 << collision.gameObject.layer) verschiebt das Bit 1 um so viele Stellen nach links,
        // wie es die Layer-ID des kollidierenden Objekts angibt. Funktioniert nur, weil die Layer intern als
        // 2er Potenzen dargestellt werden
        if ((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            this.rb.linearVelocity = Vector2.zero;
            DynamiteExplode();
            collisionObj = collision;
            OnDynamiteExplosionShockwave?.Invoke(this.dynamiteExplosionPoint, collisionObj);
        }
    }

    protected Collider2D collisionObj;
    public void ExplosionDamage()
    {
        OnDynamiteExplosion?.Invoke(this.dynamiteExplosionPoint, collisionObj);
        this.collisionObj = null;
    }





    //~~~~~~~~~~~~~~~~~~~~~~ Gizmos ~~~~~~~~~~~~~~~~~~~~~~
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(this.dynamiteExplosionPoint.position, this.Config.DamageRadius);
    }
}
