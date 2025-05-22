using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Enemy_Movement : MonoBehaviour
{

    //######################## Membervariablen ##############################
    public float speed;
    private bool isChasing;
    public int facingDirection = 1;

    private Rigidbody2D rb;
    private Transform playerTransform;
    public Animator animator;




    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // wir weisen den Rigidbody vom eigenen Objekt uns zu
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // auf anderen Charakter zulaufen:
        if (this.isChasing)
        {
            // Richtungsvektor
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
    }


    private void FixedUpdate()
    {

        // Tasten-Input, der in den Einstellungen konfiguriert wurde
        float horizontal = rb.linearVelocity.x;
        float vertical = rb.linearVelocity.y;

        // horizontal > 0 --> nach rechts laufen, aber Bild links ausgerichtet
        // horizontal < 0 --> nach links laufen, aber Bild rechts ausgerichtet
        if (horizontal > 0 && transform.localScale.x < 0 ||
            horizontal < 0 && transform.localScale.x > 0)
        {
            Flip();
        }

        animator.SetFloat("horizontal", Mathf.Abs(horizontal));
        animator.SetFloat("vertical", Mathf.Abs(vertical));

    }



    //########################### Methoden #############################
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(this.playerTransform == null)
            {
                this.playerTransform = collision.transform;
            }
            this.isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.linearVelocity = Vector2.zero;
            this.isChasing = false;
        }
    }



    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

}
