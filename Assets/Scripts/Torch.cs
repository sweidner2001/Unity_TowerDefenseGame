using Assets.Scripts;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static UnityEditor.Searcher.SearcherWindow.Alignment;



public class Torch : MonoBehaviour
{

    //######################## Membervariablen ##############################
    public Rigidbody2D rb;
    protected float attackCooldownTimer;

    // Gegner Detektion: 
    protected Transform enemyDetectionPoint;
    protected Transform detectedEnemy;
    //protected TowerHomePoint homePoint;
    protected HomePoint homePoint;

    // Zustand + Eigenschaften
    public ConfigTorch ConfigTorch { get; set; }


    // Animation:
    protected Animator animator;
    private Dictionary<SoldierState, string> stateToAnimation = new Dictionary<SoldierState, string>()
    {
        { SoldierState.Idle, "isIdling" },
        { SoldierState.Chase, "isMoving" },
        { SoldierState.Attack, "isAttacking" },
        { SoldierState.BackToTower, "isMoving" },
        { SoldierState.OnTower, "isIdling" }
        // SoldierState.Knockback hat keine Animation
    };

    private SoldierState _state;
    public SoldierState State
    {
        get => _state;
        protected set
        {
            // Alte Animation deaktivieren
            if (stateToAnimation.ContainsKey(_state))
                animator.SetBool(stateToAnimation[_state], false);

            _state = value;

            // Neue Animation aktivieren
            if (stateToAnimation.ContainsKey(_state))
                animator.SetBool(stateToAnimation[_state], true);
        }
    }


    //########################### Geerbte Methoden #############################
    void Start()
    {
        this.rb = GetComponentInParent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        this.enemyDetectionPoint = transform.Find("EnemyDetectionPoint");
        this.ConfigTorch = Resources.Load<ConfigTorch>("Config/Torch/Torch_Std");

        try
        {
            if (ConfigTorch == null)
                throw new Exception("Variable ConfigTorch = null");
            if (enemyDetectionPoint == null)
                throw new Exception("Variable enemyDetectionPoint = null");

            InitHealth(this.ConfigTorch.maxHealth);
            this.homePoint = GetComponent<HomePoint>();
            this.homePoint?.Init();
            //InitHomePoint();
            ChangeState(SoldierState.BackToTower);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.ToString());
        }
    }




    // Update is called once per frame
    void Update()
    {
        if (this.State == SoldierState.Knockback)
        {
            return;
        }

        try
        {
            CheckForPlayer();
            if (this.attackCooldownTimer > 0)
                this.attackCooldownTimer -= Time.deltaTime;


            switch (this.State)
            {
                case SoldierState.Chase:
                    ChaseEnemy();
                    break;
                case SoldierState.Attack:
                    Attack();
                    break;
                case SoldierState.BackToTower:
                    this.homePoint?.GoBackToTower();
                    break;
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }




    //########################### Methoden #############################
    protected void InitHealth(int maxHealth)
    {
        // Health-Objekt initialisieren:
        Health health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("Health-Komponente nicht gefunden!");
            return;
        }
        health.Init(maxHealth);
    }



    //~~~~~~~~~~~~~~~~~~~~~~~ Zustandswechsel ~~~~~~~~~~~~~~~~~~~~~~~~~~
    public void ChangeState(SoldierState newState)
    {
        this.State = newState;
    }



    /// <summary>
    /// Damit der Gegner auch verfolgt wird, wenn er sich im Verfolger-Range befindet, nachdem der Angriff erfolgt ist.
    /// </summary>
    /// <param name="collision"></param>
    private void CheckForPlayer()
    {
        // Alle Gegner detektieren:
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.enemyDetectionPoint.position,
                                                        this.ConfigTorch.playerDetectionRange,
                                                        this.ConfigTorch.detectionLayer);

        //**************** Gegner gefunden ****************
        if (hits.Length > 0)
        {

            this.detectedEnemy = hits[0].transform;

            //-------------- Gegner angreifen ------------------
            // wenn sich ein Gegner in der Attack-Range befindet und der Cooldown abgelaufen ist 
            float enemyDistance = Vector2.Distance(this.transform.position, this.detectedEnemy.position);
            if (enemyDistance <= this.ConfigTorch.maxAttackRange && this.attackCooldownTimer <= 0)
            {
                // Angreifen:
                this.attackCooldownTimer = this.ConfigTorch.attackCooldown;
                ChangeState(SoldierState.Attack);

                // Nach den Angriff wird in der Animation wieder in den "IDle" Status gewechselt
            }
            if (enemyDistance <= this.ConfigTorch.maxAttackRange && this.State == SoldierState.Chase)
            {
                // Vor Gegner stehen bleiben, wenn er sich in der Attack-Range befindet:
                this.rb.linearVelocity = Vector2.zero;
                ChangeState(SoldierState.Idle);
                Debug.Log("Gegner gefunden - #Stehen bleiben");
            }
            //-------------- Auf Gegner zulaufen ----------------
            // eine begonenne Attacke soll zuerst zu Ende laufen
            else if (enemyDistance > this.ConfigTorch.maxAttackRange && this.State != SoldierState.Attack)
            {
                ChangeState(SoldierState.Chase);
                //Debug.Log("Gegner gefunden - hinlaufen");
            }

        }
        //**************** keinen Gegner gefunden ****************
        else
        {
            if (this.homePoint != null && this.State != SoldierState.OnTower)
            {
                ChangeState(SoldierState.BackToTower);
            }
            else if (this.State != SoldierState.OnTower)
            {
                // Stehen bleiben, kein Gegner gefunden
                this.rb.linearVelocity = Vector2.zero;
                ChangeState(SoldierState.Idle);
            }
        }
    }



    //~~~~~~~~~~~~~~~~~~~~~ Angreifen ~~~~~~~~~~~~~~~~~~~~~~~
    public void Attack()
    {
        // zum Angreifen stehen bleiben, Attacke wird durch den Zustandswechsel ausgelöst.
        // Die weitere Logik befindet sich in der Animation und in Enemy_Combat.cs
        //Debug.Log("Attacking player now");
        this.rb.linearVelocity = Vector2.zero;
    }
    public void ChaseEnemy()
    {
        Move(this.detectedEnemy);
    }





    //~~~~~~~~~~~~~~~~~~~~~~~ Movement ~~~~~~~~~~~~~~~~~~~~~~~~~~

    public void Move(Transform destinationTransform)
    {
        // Richtungsvektor
        Vector2 direction = (destinationTransform.position - this.transform.position).normalized;
        this.rb.linearVelocity = direction * this.ConfigTorch.movingSpeed;

        // aktuelle Bewegung abrufen
        FlipCharakterIfNecessary(this.rb.linearVelocity.x);
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





    //~~~~~~~~~~~~~~~~~~~~~ Rendering ~~~~~~~~~~~~~~~~~~~~~~~
    /// <summary>
    /// Zeichnet den Detection Point mit Radius für Gegnerische Figuren
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.enemyDetectionPoint.position, this.ConfigTorch.playerDetectionRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, this.ConfigTorch.maxAttackRange);
    }

}