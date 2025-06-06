using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{


    //######################## Membervariablen ##############################
    private Rigidbody2D rb;

    // Eigenschaften des Pfeils
    public Vector2 ArrowDirection { set; get; } = Vector2.right;
    public float maxLifeSpan = 5;                      // Max. Lebenszeit in s des Pfeils
    public float LifeSpanOnHittetObject { get; set; } = 2;                      // Max. Lebenszeit in s des Pfeils

    // Welche GameObjekts kann ich treffen? (müssen Colllider besitzen)
    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;

    // Sprite wenn Gegner getroffen:
    private SpriteRenderer sr;
    public Sprite objectHitSprite;

    public float ArrowSpeed { get; set; } = 6;                // Geschwindigkeit des Pfeils


    // Action-Delegate statt eigener Delegate-Definition
    public event Action<Collision2D> OnEnemyArrowCollision;



    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();   
        this.sr = GetComponent<SpriteRenderer>();

        rb.linearVelocity = this.ArrowDirection * this.ArrowSpeed;
        RatateArrow();
        Destroy(gameObject, maxLifeSpan);
    }



    // Update is called once per frames
    void Update()
    {
        
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


    private void RatateArrow()
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
            Destroy(gameObject, LifeSpanOnHittetObject);
        }
        else if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Pfeil soll auch an anderen Objekten hängen bleiben
            AttachToTarget(collision.gameObject.transform);
            Destroy(gameObject, LifeSpanOnHittetObject);
        }
    }

    private void AttachToTarget(Transform target)
    {
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
        transform.SetParent(target);
    }


}
