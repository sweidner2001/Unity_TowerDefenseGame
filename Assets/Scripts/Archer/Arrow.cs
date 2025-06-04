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
        if((enemyLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.ChangeHealth(-damage);
            collision.gameObject.GetComponent<PlayerMovement>()?.Knockback(forceTransform: this.transform, knockbackForce, stunTime);
        }
    }


}
