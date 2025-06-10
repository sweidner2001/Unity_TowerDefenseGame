using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoldierBase : MonoBehaviour
{

    //######################## Membervariablen ##############################
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Transform enemyDetectionPoint;
    protected Transform detectedEnemy;

    public ConfigSoldierBase Config { get; set; }
    protected float attackCooldownTimer;

    protected Dictionary<SoldierState, string> stateToAnimation = new Dictionary<SoldierState, string>()
    {
        { SoldierState.Idle, "isIdling" },
        { SoldierState.SeeEnemy, "isMoving" },
        { SoldierState.Attack, "isAttacking" },
        { SoldierState.SeeNoEnemy, "isMoving" },
        { SoldierState.OnTower, "isIdling" }
    };

    private SoldierState _state;
    public virtual SoldierState State
    {
        get => _state;
        protected set
        {
            if (stateToAnimation.ContainsKey(_state))
                animator.SetBool(stateToAnimation[_state], false);

            _state = value;

            if (stateToAnimation.ContainsKey(_state))
                animator.SetBool(stateToAnimation[_state], true);
        }
    }




    //########################### Geerbte Methoden #############################
    protected virtual void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyDetectionPoint = transform.Find("EnemyDetectionPoint");
    }

    protected virtual void Update()
    {
        if (State == SoldierState.Knockback)
            return;

        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;

        CheckForPlayer();
        switch (State)
        {
            case SoldierState.SeeEnemy:
                ChaseEnemy();
                break;
            case SoldierState.Attack:
                Attack();
                break;
            case SoldierState.SeeNoEnemy:
                GoBackToTower();
                break;
        }
    }




    //########################### Methoden #############################
    protected virtual void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(enemyDetectionPoint.position, Config.playerDetectionRange, Config.detectionLayer);
        if (hits.Length > 0)
        {
            detectedEnemy = hits[0].transform;
            float enemyDistance = Vector2.Distance(transform.position, detectedEnemy.position);
            if (enemyDistance <= Config.maxAttackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = Config.attackCooldown;
                ChangeState(SoldierState.Attack);
            }
            else if (enemyDistance > Config.maxAttackRange && State != SoldierState.Attack)
            {
                ChangeState(SoldierState.SeeEnemy);
            }
        }
        else
        {
            ChangeState(SoldierState.SeeNoEnemy);
        }
    }

    public virtual void ChangeState(SoldierState newState)
    {
        State = newState;
    }

    public virtual void Attack()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public virtual void ChaseEnemy()
    {
        if (detectedEnemy != null)
            Move(detectedEnemy);
    }

    public virtual void GoBackToTower()
    {
    }


    //~~~~~~~~~~~~~~~~~~~~~~~ Movement ~~~~~~~~~~~~~~~~~~~~~~~
    protected virtual void Move(Transform destinationTransform)
    {
        Vector2 direction = (destinationTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * Config.movingSpeed;
        FlipCharakterIfNecessary(rb.linearVelocity.x);
    }



    protected virtual void FlipCharakterIfNecessary(float horizontalMovement)
    {
        if (horizontalMovement > 0 && transform.localScale.x < 0 ||
            horizontalMovement < 0 && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    protected virtual void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }






}