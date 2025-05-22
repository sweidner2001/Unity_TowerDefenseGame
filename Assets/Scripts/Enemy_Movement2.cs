using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public enum EnemyState : int
{
    Idle,
    Moving
}


public class Enemy_Movement2 : MonoBehaviour
{

    //######################## Membervariablen ##############################
    public float speed = 1;
    public int facingDirection = 1;

    private EnemyState enemyState;
    private Rigidbody2D rb;
    private Transform playerTransform;
    private Animator animator;




    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // wir weisen den Rigidbody vom eigenen Objekt uns zu
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        // auf anderen Charakter zulaufen:
        if (this.enemyState == EnemyState.Moving)
        {
            // Richtungsvektor
            Vector2 direction = (this.playerTransform.position - this.transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }
    }


    private void FixedUpdate()
    {

        // aktuelle Bewegung abrufen
        float horizontal = rb.linearVelocity.x;
        float vertical = rb.linearVelocity.y;

        // horizontal > 0 --> nach rechts laufen, aber Bild links ausgerichtet
        // horizontal < 0 --> nach links laufen, aber Bild rechts ausgerichtet
        if (horizontal > 0 && this.transform.localScale.x < 0 ||
            horizontal < 0 && this.transform.localScale.x > 0)
        {
            Flip();
        }

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
            ChangeState(newState: EnemyState.Moving);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(newState: EnemyState.Idle);
        }
    }

    private void ChangeState(EnemyState newState)
    {
        // Exit old state
        if (enemyState == EnemyState.Idle)
            animator.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Moving)
            animator.SetBool("isMoving", false);

        // Set new state
        if (newState == EnemyState.Idle)
            animator.SetBool("isIdle", true);
        else if (newState == EnemyState.Moving)
            animator.SetBool("isMoving", true);

        this.enemyState = newState;
    }



    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

}
