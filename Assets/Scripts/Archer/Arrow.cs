using UnityEngine;

public class Arrow : MonoBehaviour
{


    //######################## Membervariablen ##############################
    public Rigidbody2D rb;
    public Vector2 arrowDirection = Vector2.right;
    public float lifeSpan = 2;                      // Lebenszeit in s des Pfeils
    public float speed;
    public int damage;

    public float knockbackForce = 3;
    public float knockbackTime = 0.15f;
    public float stunTime = 0.2f;


    public LayerMask enemyLayer;
    public LayerMask obstacleLayer;

    public SpriteRenderer sr;
    public Sprite buriedSprite;


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();   
        rb.linearVelocity = this.arrowDirection * speed;
        RatateArrow();
        Destroy(gameObject, lifeSpan);
    }

    // Update is called once per frames
    void Update()
    {
        
    }




    //########################### Methoden #############################
    private void RatateArrow()
    {
        // Winkel zwischen 2 Punkten
        float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;

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
            collision.gameObject.GetComponent<PlayerHealth>()?.ChangeHealth(-damage);
            collision.gameObject.GetComponent<PlayerMovement>()?.Knockback(forceTransform: this.transform, knockbackForce, stunTime);
            AttachToTarget(collision.gameObject.transform);
        }
        else if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            // Pfeil soll auch an anderen Objekten hängen bleiben
            AttachToTarget(collision.gameObject.transform);
        }
    }

    private void AttachToTarget(Transform target)
    {
        // 1. Bild austauschen:
        sr.sprite = buriedSprite;

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
