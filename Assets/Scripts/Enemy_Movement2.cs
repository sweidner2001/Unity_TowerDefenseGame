using JetBrains.Annotations;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public enum EnemyState : int
{
    Idle,
    Move,
    Attack,
    Knockback
}


public class Enemy_Movement2 : MonoBehaviour
{

    //######################## Membervariablen ##############################
    // Bewegung:
    public float moveSpeed = 1;

    // Attacke:
    public float attackRange = 0.7f;
    public float attackCooldown = 2;
    private float attackCooldownTimer;

    // Gegner Detektion: 
    public float playerDetectionRange = 1;
    public Transform detectionPoint;
    public LayerMask detectionLayer;            // was wollen wir detektieren?
    private Transform playerTransform;          // Transform-Attr. des detektierten Objektes



    private EnemyState enemyState;
    private Rigidbody2D rb;
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
        if(this.enemyState == EnemyState.Knockback)
        {
            return;
        }

        CheckForPlayer();
        if(this.attackCooldownTimer > 0)
        {
            this.attackCooldownTimer -= Time.deltaTime;
        }


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
        // Richtungsvektor
        Vector2 direction = (this.playerTransform.position - this.transform.position).normalized;
        this.rb.linearVelocity = direction * moveSpeed;

        // aktuelle Bewegung abrufen
        FlipCharakter(this.rb.linearVelocity.x);
    }


    public void Attack()
    {
        // zum Angreifen stehen bleiben, Attacke wird durch den Zustandswechsel ausgelöst.
        // Die weitere Logik befindet sich in der Animation und in Enemy_Combat.cs
        //Debug.Log("Attacking player now");
        this.rb.linearVelocity = Vector2.zero;
    }


    /// <summary>
    /// Damit der Gegner auch verfolgt wird, wenn er sich im Verfolger-Range befindet, nachdem der Angriff erfolgt ist.
    /// </summary>
    /// <param name="collision"></param>
    private void CheckForPlayer()
    {
        // Alle Gegner detektieren:
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.detectionPoint.position, this.playerDetectionRange, this.detectionLayer);

        if (hits.Length > 0) {
            
            this.playerTransform = hits[0].transform;

            //-------------- Gegner angreifen ------------------
            // wenn sich ein Gegner in der Attack-Range befindet und der Cooldown abgelaufen ist 
            float enemyDistance = Vector2.Distance(this.transform.position, this.playerTransform.position);
            if (enemyDistance <= this.attackRange && this.attackCooldownTimer <= 0)
            {
                // Angreifen:
                this.attackCooldownTimer = this.attackCooldown;
                ChangeState(EnemyState.Attack);

                // Nach den Angriff wird in der Animation wieder in den "IDle" Status gewechselt
            }
            if (enemyDistance <= this.attackRange && enemyState == EnemyState.Move)
            {
                // Vor Gegner stehen bleiben, wenn er sich in der Attack-Range befindet:
                this.rb.linearVelocity = Vector2.zero;
                ChangeState(EnemyState.Idle);
                Debug.Log("Gegner gefunden - #Stehen bleiben");
            }
            //-------------- Auf Gegner zulaufen ----------------
            // eine begonenne Attacke soll zuerst zu Ende laufen
            else if(enemyDistance > this.attackRange && enemyState != EnemyState.Attack)
            {
                ChangeState(EnemyState.Move);
                Debug.Log("Gegner gefunden - hinlaufen");
            }

        }
        else
        {
            // Stehen bleiben, kein Gegner gefunden
            this.rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
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


    /// <summary>
    /// Dreht das Sprite Bild um 180°, wenn die Figur beim Gehen die Richtung wechselt
    /// </summary>
    /// <param name="horizontalMovement">relative Bewegungsrichtung in X-Achse von der Figur aus gesehen</param>
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


    /// <summary>
    /// Dreht das Sprite Bild um 180°
    /// </summary>
    protected void Flip()
    {
        this.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }






    /// <summary>
    /// Zeichnet den Detection Point mit Radius für Gegnerische Figuren
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.detectionPoint.position, this.playerDetectionRange);
    }

}
