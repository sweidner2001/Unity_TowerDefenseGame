using JetBrains.Annotations;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public enum EnemyState : int
{
    Idle,
    Chase,
    Attack,
    Knockback,
    BackToTower,
    OnTower
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


    public HomePoint homePoint;
    


    private EnemyState enemyState;
    private Rigidbody2D rb;
    private Animator animator;




    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // wir weisen den Rigidbody vom eigenen Objekt uns zu
        this.rb = GetComponentInParent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        ChangeState(EnemyState.BackToTower);
        try
        {
            InitHomePoint();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        InitHealth();
    }

    private void InitHealth()
    {
        // Health-Objekt initialisieren:
        Health health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("Health-Komponente nicht gefunden!");
            return;
        }
        health.Init(6);
    }


    // Update is called once per frame
    void Update()
    {
        if(this.enemyState == EnemyState.Knockback)
        {
            return;
        }

        try
        {
            CheckForPlayer();
            if(this.attackCooldownTimer > 0)
            {
                this.attackCooldownTimer -= Time.deltaTime;
            }


            switch(this.enemyState)
            {
                case EnemyState.Chase:
                    // auf anderen Charakter zulaufen:
                    ChaseEnemy();
                    break;
                case EnemyState.Attack:
                    Attack();
                    break;
                case EnemyState.BackToTower:
                    GoBackToTower();
                    break;
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }


    private void FixedUpdate()
    {

    }



    //########################### Methoden #############################


    public void ChaseEnemy()
    {
        Move(this.playerTransform);
    }

    private void Move(Transform destinationTransform)
    {
        // Richtungsvektor
        Vector2 direction = (destinationTransform.position - this.transform.position).normalized;
        this.rb.linearVelocity = direction * this.moveSpeed;

        // aktuelle Bewegung abrufen
        FlipCharakterIfNecessary(this.rb.linearVelocity.x);
    }


    protected void InitHomePoint()
    {
        if (this.homePoint == null)
        {
            this.homePoint = FindNearestAvailableHomePoint().GetComponent<HomePoint>();

            // Notfalls einfach stehen bleiben, wenn kein freier HomePoint existiert
            if (this.homePoint == null)
            {
                ChangeState(EnemyState.Idle);
                throw new Exception("Kein Homepoint gefunden");
            }
            ChangeHomePointState(true);
        }
    }

    public void GoBackToTower()
    {
        if (this.homePoint == null)
        {
            InitHomePoint();
        }

        Move(this.homePoint.transform);
        if((homePoint.transform.position - this.transform.position).magnitude <= this.homePoint.homePointRadius)
        {
            // ich befinde mich an meinen HomePoint
            this.rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.OnTower);

        }
    }

    public void ChangeHomePointState(bool isAssigned)
    {
        if (this.homePoint != null) {
            this.homePoint.isAssigned = isAssigned;
        }
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

        //~~~~~~~~~~~~~~~~~ Gegner gefunden ~~~~~~~~~~~~~~~~~~~~~
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
            if (enemyDistance <= this.attackRange && enemyState == EnemyState.Chase)
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
                ChangeState(EnemyState.Chase);
                //Debug.Log("Gegner gefunden - hinlaufen");
            }

        }
        //~~~~~~~~~~~~~~~~~ keinen Gegner gefunden ~~~~~~~~~~~~~~~~~~~~~
        else
        {
            if(this.homePoint != null && this.enemyState != EnemyState.OnTower)
            {
                ChangeState(EnemyState.BackToTower);
            }
            else if(this.enemyState != EnemyState.OnTower)
            {
                // Stehen bleiben, kein Gegner gefunden
                this.rb.linearVelocity = Vector2.zero;
                ChangeState(EnemyState.Idle);
            }
        }


    }
    
    public void ChangeState(EnemyState newState)
    {
        // Exit old state
        if (this.enemyState == EnemyState.Idle)
            animator.SetBool("isIdling", false);
        else if (this.enemyState == EnemyState.Chase)
            animator.SetBool("isMoving", false);
        else if (this.enemyState == EnemyState.Attack)
            animator.SetBool("isAttacking", false);
        else if (this.enemyState == EnemyState.BackToTower)
            animator.SetBool("isMoving", false);
        else if (this.enemyState == EnemyState.OnTower)
            animator.SetBool("isIdling", false);
        //else if (this.enemyState == EnemyState.Knockback)
        //{
        //    // Es gibt keine Animation für Knockout
        //}

        //Debug.Log("CurrentState:" + newState);
        // Set new state
        this.enemyState = newState;

        if (this.enemyState == EnemyState.Idle)
            animator.SetBool("isIdling", true);
        else if (this.enemyState == EnemyState.Chase)
            animator.SetBool("isMoving", true);
        else if (this.enemyState == EnemyState.Attack)
            animator.SetBool("isAttacking", true);
        else if (this.enemyState == EnemyState.BackToTower)
            // wir gehen zurück zum Turm
            animator.SetBool("isMoving", true);
        else if (this.enemyState == EnemyState.OnTower)
            // wir warten am Turm
            animator.SetBool("isIdling", true);
        //else if (this.enemyState == EnemyState.Knockback)
        //{
        //    // Auto-Wechsel in Standard-Status
        //}


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






    private Transform FindNearestAvailableHomePoint()
    {
        HomePoint[] homePoints = transform.parent.parent.GetComponentsInChildren<HomePoint>();

        if (homePoints.Length == 0)
        {
            Debug.LogWarning("Kein HomePoints gefunden!");
        }

        // Den nächstgelegenen freien HomePoint finden
        var nearestPoint = homePoints
            .Where(point => point.isAssigned == false) // Nur unbesetzte HomePoints
            .OrderBy(point => Vector3.Distance(this.transform.position, point.transform.position)).FirstOrDefault(); // Kürzeste Distanz finden

        if (nearestPoint == null)
        {
            Debug.LogWarning("Kein HomePoint gefunden!!!!!");
        }

        return nearestPoint?.transform; // Rückgabe des Transforms oder null

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
