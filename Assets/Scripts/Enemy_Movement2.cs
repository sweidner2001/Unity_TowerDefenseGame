using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public enum EnemyState : int
{
    Idle,
    Move,
    Attack
}


public class Enemy_Movement2 : MonoBehaviour
{

    //######################## Membervariablen ##############################
    public float speed = 1;
    public float attackRange = 0.7f;
    public float attackCooldown = 2;

    private EnemyState enemyState;

    private Rigidbody2D rb;
    private Transform playerTransform;
    private Animator animator;




    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // wir weisen den Rigidbody vom eigenen Objekt uns zu
        this.rb = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        switch(this.enemyState)
        {
            case EnemyState.Move:
                // auf anderen Charakter zulaufen:
                Moving();
                break;
            case EnemyState.Attack:
                Attack();
                break;

        }

    }


    private void FixedUpdate()
    {

    }



    //########################### Methoden #############################
    public void Moving()
    {
        if (Vector2.Distance(this.transform.position, this.playerTransform.position) <= this.attackRange)
        {
            ChangeState(EnemyState.Attack);
        }

        // Richtungsvektor
        Vector2 direction = (this.playerTransform.position - this.transform.position).normalized;
        this.rb.linearVelocity = direction * speed;

        // aktuelle Bewegung abrufen
        FlipCharakter(this.rb.linearVelocity.x);
    }


    public void Attack()
    {
        Debug.Log("Attacking player now");
        rb.linearVelocity = Vector2.zero;
    }


    /// <summary>
    /// Damit der Gegner auch verfolgt wird, wenn er sich im Verfolger-Range befindet, nachdem der Angriff erfolgt ist.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(this.playerTransform == null)
            {
                this.playerTransform = collision.transform;
            }
            ChangeState(newState: EnemyState.Move);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.rb.linearVelocity = Vector2.zero;
            ChangeState(newState: EnemyState.Idle);
        }
    }

    public void ChangeState(EnemyState newState)
    {
        // Exit old state
        if (enemyState == EnemyState.Idle)
            animator.SetBool("isIdling", false);
        else if (enemyState == EnemyState.Move)
            animator.SetBool("isMoving", false);
        else if (enemyState == EnemyState.Attack)
            animator.SetBool("isAttacking", false);

        // Set new state
        this.enemyState = newState;
        if (this.enemyState == EnemyState.Idle)
            animator.SetBool("isIdling", true);
        else if (this.enemyState == EnemyState.Move)
            animator.SetBool("isMoving", true);
        else if (this.enemyState == EnemyState.Attack)
            animator.SetBool("isAttacking", true);

        
    }



    protected void FlipCharakter(float horizontalMovement)
    {
        // horizontal > 0 --> nach rechts laufen, aber Bild links ausgerichtet
        // horizontal < 0 --> nach links laufen, aber Bild rechts ausgerichtet
        if (horizontalMovement > 0 && this.transform.localScale.x < 0 ||
            horizontalMovement < 0 && this.transform.localScale.x > 0)
        {
            Flip();
        }
    }
    protected void Flip()
    {
        this.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

}
