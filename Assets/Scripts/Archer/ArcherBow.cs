using UnityEngine;

public class ArcherBow : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public Transform arrowLaunchPoint;

    // Unser Schuss-Objekt (Pfeil)
    public GameObject arrowPrefab;

    private Vector2 aimDirection = Vector2.right;


    // Gegner Detektion: 
    public float playerDetectionRange = 4f;
    public Transform detectionPoint;
    public LayerMask detectionLayer;            // was wollen wir detektieren?
    private Transform aimTransform;          // Transform-Attr. des detektierten Objektes


    public float attackCooldown = 2;
    private float attackTimer;


    //########################### Geerbte Methoden #############################
    void Start()
    {
        
        
    }

    void Update()
    {

        attackTimer -= Time.deltaTime;


        HandleAiming();

        if (this.aimTransform != null && this.attackTimer <= 0) // Input.GetButtonDown("UserAttack")
        {
            Shoot();
        }
        
    }



    private void HandleAiming()
    {
        // Alle Gegner detektieren:
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.detectionPoint.position, this.playerDetectionRange, this.detectionLayer);

        if (hits.Length > 0)
        {
            this.aimTransform = hits[0].transform;
            this.aimDirection = (this.aimTransform.position - this.transform.position).normalized;
            FlipCharakterIfNecessary(this.aimDirection.x);
        }
        else
        {
            this.aimTransform = null;
        }
    }
     
    public void Shoot()
    {
        Arrow arrow = Instantiate(arrowPrefab, arrowLaunchPoint.position, Quaternion.identity).GetComponent<Arrow>();
        arrow.arrowDirection = aimDirection;
        this.attackTimer = this.attackCooldown;
    }





    /// <summary>
    /// Dreht das Sprite Bild um 180°, wenn die Figur beim Gehen die Richtung wechselt
    /// </summary>
    /// <param name="horizontalMovement">relative Bewegungsrichtung in X-Achse von der Figur aus gesehen</param>
    protected void FlipCharakterIfNecessary(float horizontalMovement)
    {
        // horizontal > 0 --> nach rechts laufen, aber Bild links ausgerichtet
        // horizontal < 0 --> nach links laufen, aber Bild rechts ausgerichtet
        if (horizontalMovement > 0 && this.transform.localScale.x < 0 ||
            horizontalMovement < 0 && this.transform.localScale.x > 0)
        {
            Flip();
        }
    }


    /// <summary>
    /// Dreht das Sprite Bild um 180°
    /// </summary>
    protected void Flip()
    {
        this.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.detectionPoint.position, this.playerDetectionRange);
    }

}
