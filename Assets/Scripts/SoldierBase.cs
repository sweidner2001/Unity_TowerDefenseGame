using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBase : MonoBehaviour
{
 



    protected Rigidbody2D rb;
    protected Animator animator;
    protected Transform enemyDetectionPoint;
    protected Transform detectedEnemy;

    public ConfigTorch ConfigTorch { get; set; }
    protected float attackCooldownTimer;

    private Dictionary<SoldierState, string> stateToAnimation = new Dictionary<SoldierState, string>()
    {
        { SoldierState.Idle, "isIdling" },
        { SoldierState.Chase, "isMoving" },
        { SoldierState.Attack, "isAttacking" },
        { SoldierState.BackToTower, "isMoving" },
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

    protected virtual void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyDetectionPoint = transform.Find("EnemyDetectionPoint");
        ConfigTorch = Resources.Load<ConfigTorch>("Config/Torch/Torch_Std");
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
            case SoldierState.Chase: ChaseEnemy(); break;
            case SoldierState.Attack: Attack(); break;
// case SoldierState.BackToTower: GoBackToTower(); break;
        }
    }

    protected virtual void CheckForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(enemyDetectionPoint.position, ConfigTorch.playerDetectionRange, ConfigTorch.detectionLayer);
        if (hits.Length > 0)
        {
            detectedEnemy = hits[0].transform;
            float enemyDistance = Vector2.Distance(transform.position, detectedEnemy.position);
            if (enemyDistance <= ConfigTorch.maxAttackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = ConfigTorch.attackCooldown;
                ChangeState(SoldierState.Attack);
            }
            else if (enemyDistance > ConfigTorch.maxAttackRange && State != SoldierState.Attack)
            {
                ChangeState(SoldierState.Chase);
            }
        }
        else
        {
            ChangeState(SoldierState.BackToTower);
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

    protected virtual void Move(Transform destinationTransform)
    {
        Vector2 direction = (destinationTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * ConfigTorch.movingSpeed;
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
