using UnityEngine;

public class ShotGunBullet : MonoBehaviour
{
    //public Vector2 ArrowDirection { set; get; } = Vector2.right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //this.rb = GetComponent<Rigidbody2D>();
        //this.sr = GetComponent<SpriteRenderer>();


        //rb.gravityScale = 0;
        //RotateArrowBeforeAttack();

        //if (this.enemyTransform != null)
        //{
        //    StartCoroutine(FlyAlongBezierDynamic());
        //}
        //rb.linearVelocity = this.ArrowDirection * this.ArrowSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //private float timer = 3f;

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
    //private void RotateArrowBeforeAttack2()
    //{
    //    float angle = Mathf.Atan2(ArrowDirection.y, ArrowDirection.x) * Mathf.Rad2Deg;
    //    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    //}


}
