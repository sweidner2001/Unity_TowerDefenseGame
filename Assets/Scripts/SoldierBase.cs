using Assets.Resources.Interfaces;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public abstract class SoldierBase<TConfig> : MonoBehaviour, ISoldierBase where TConfig : IHealth, IKnockback, IMove, IAttack
{

    //######################## Membervariablen ##############################
    public Rigidbody2D Rb { get; set; }
    protected Animator animator;
    protected Transform enemyDetectionPoint;
    protected Transform detectedEnemy;

    public TConfig Config { get; set; }
    protected float attackCooldownTimer;

    protected Dictionary<SoldierState, string> stateToAnimation = new Dictionary<SoldierState, string>()
    {
        { SoldierState.Idle, "isIdling" },
        { SoldierState.SeeEnemy, "isMoving" },
        { SoldierState.SeeNoEnemy, "isMoving" },
        { SoldierState.Attack, "isAttacking" },
        { SoldierState.OnTower, "isIdling" },
        { SoldierState.Dead, "isDead" },
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
        Rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyDetectionPoint = transform.Find("EnemyDetectionPoint");
    }

    public abstract TConfig GetConfig();

    //protected virtual void Update()
    //{
    //    if (State == SoldierState.Knockback)
    //        return;

    //    if (attackCooldownTimer > 0)
    //        attackCooldownTimer -= Time.deltaTime;

    //    CheckForPlayer();
    //    switch (State)
    //    {
    //        case SoldierState.SeeEnemy:
    //            ChaseEnemy();
    //            break;
    //        case SoldierState.Attack:
    //            Attack();
    //            break;
    //        case SoldierState.SeeNoEnemy:
    //            GoBackToTower();
    //            break;
    //    }
    //}




    //########################### Methoden #############################
    public Collider2D[] GetDetectedEnemies()
    {
        return Physics2D.OverlapCircleAll(enemyDetectionPoint.position, Config.PlayerDetectionRange, Config.DetectionLayer);
    }

    //protected virtual void CheckForPlayer()
    //{
    //    Collider2D[] hits = GetDetectedEnemies();
    //    if (hits.Length > 0)
    //    {
    //        detectedEnemy = hits[0].transform;
    //        float enemyDistance = Vector2.Distance(transform.position, detectedEnemy.position);
    //        if (enemyDistance <= Config.maxAttackRange && attackCooldownTimer <= 0)
    //        {
    //            attackCooldownTimer = Config.attackCooldown;
    //            ChangeState(SoldierState.Attack);
    //        }
    //        else if (enemyDistance > Config.maxAttackRange && State != SoldierState.Attack)
    //        {
    //            ChangeState(SoldierState.SeeEnemy);
    //        }
    //    }
    //    else
    //    {
    //        ChangeState(SoldierState.SeeNoEnemy);
    //    }
    //}

    public virtual void ChangeState(SoldierState newState)
    {
        if(State == SoldierState.Dead)
            return; // Keine Änderung des States, daher nichts tun
        
        State = newState;
    }

    //public virtual void Attack()
    //{
    //    rb.linearVelocity = Vector2.zero;
    //}

    //public virtual void ChaseEnemy()
    //{
    //    if (detectedEnemy != null)
    //        Move(detectedEnemy);
    //}

    //public virtual void GoBackToTower()
    //{
    //}


    //~~~~~~~~~~~~~~~~~~~~~~~ Movement ~~~~~~~~~~~~~~~~~~~~~~~
    public virtual void Move(Transform destinationTransform)
    {
        Vector2 direction = (destinationTransform.position - transform.position).normalized;
        Rb.linearVelocity = direction * Config.MovingSpeed;
        FlipCharakterIfNecessary(Rb.linearVelocity.x);
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





    //~~~~~~~~~~~~~~~~~~~~~ Rendering ~~~~~~~~~~~~~~~~~~~~~~~
    /// <summary>
    /// Zeichnet den Detection Point mit Radius für Gegnerische Figuren
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.enemyDetectionPoint.position, this.Config.PlayerDetectionRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(this.transform.position, this.Config.MaxAttackRange);
    }


}