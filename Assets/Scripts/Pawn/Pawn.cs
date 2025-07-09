using Assets.Scripts;
using System;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.STP;

public class Pawn : SoldierBase<ConfigPawn>
{
    //######################## Membervariablen ##############################
    protected HomePoint homePoint;




    //########################### Geerbte Methoden #############################
    protected override void Start()
    {

        base.Start();
        this.Config = GetConfig();
        try
        {
            if (Config == null)
                throw new Exception("Variable ConfigTorch = null");
            if (enemyDetectionPoint == null)
                throw new Exception("Variable enemyDetectionPoint = null");

            InitHealth(this.Config.MaxHealth);
            //this.homePoint = GetComponent<HomePoint>();
            //this.homePoint?.Init();
            //InitHomePoint();
            ChangeState(SoldierState.SeeNoEnemy);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.ToString());
        }

        movingPath = MovingPath.Instance;

        currentPathCheckpointIdx = 0;
        targetPathCheckpoint = movingPath.GetWaypointPosition(0);
    }

    private void Awake()
    {
        
    }


    private void OnEnable()
    {

    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (this.attackCooldownTimer > 0)
            this.attackCooldownTimer -= Time.deltaTime;

        // Attacke soll zu ende laufen
        if (this.State == SoldierState.Knockback || this.State == SoldierState.Attack || this.State == SoldierState.Dead || this.State == SoldierState.Survived)
        {
            return;
        }

        try
        {
            CheckForPlayer();


            switch (this.State)
            {
                case SoldierState.SeeEnemy:
                    ChaseEnemy();
                    break;
                case SoldierState.Attack:
                    Attack();
                    break;
                case SoldierState.SeeNoEnemy:
                    GoToNextWayCheckpoint();
                    GoBackToTower();
                    break;
            }

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }




    //########################### Methoden #############################
    public override ConfigPawn GetConfig()
    {
        if (Config == null)
        {
            Config = Resources.Load<ConfigPawn>("Config/Pawn/Pawn_Std");
        }
        return Config;
    }


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








    protected MovingPath movingPath;
    protected Vector2 targetPathCheckpoint;
    protected int currentPathCheckpointIdx;





    protected void GoToNextWayCheckpoint()
    {
        MoveToPosition(targetPathCheckpoint);
        if (Vector2.Distance(transform.position, targetPathCheckpoint) < 0.1f)
        {
            if(currentPathCheckpointIdx < movingPath.Checkpoints.Count -1)
            {
                currentPathCheckpointIdx++;
                this.targetPathCheckpoint = movingPath.GetWaypointPosition(currentPathCheckpointIdx);

            } 
            else
            {
                // wir sind am Ziel!
                this.Rb.linearVelocity = Vector2.zero;
                ChangeState(SoldierState.Survived);
                //gameObject.SetActive(false);

            }
        }
    }



    protected bool CheckIfEnemyIsBehind()
    {
        // Wenn sich der Gegner im Attack-Range befindet, dann trotzdem angreifen:
        float enemyDistance = Vector2.Distance(this.transform.position, this.detectedEnemy.position);
        if (enemyDistance <= this.Config.MaxAttackRange)
        {
            return false;
        }


        // Prüfen, ob der Gegner hinter dem Pawn steht:
        Vector2 checkpointDir = ((Vector2)targetPathCheckpoint - (Vector2)transform.position).normalized;
        Vector2 toEnemy = ((Vector2)this.detectedEnemy.position - (Vector2)transform.position).normalized;

        // Prüfen, ob der Gegner "hinter" dem Pawn steht (Winkel > 90°)
        //	> 0: Beide zeigen grob in die gleiche Richtung(Winkel < 90°)
        //	< 0: Sie zeigen in entgegengesetzte Richtungen(Winkel > 90°)
        //	= 0: Sie stehen genau senkrecht zueinander(Winkel = 90°)
        float dot = Vector2.Dot(checkpointDir, toEnemy);
        bool enemyIsBehind = dot < 0;
        return enemyIsBehind;
    }


    //~~~~~~~~~~~~~~~~~~~~~~~ Zustandswechsel ~~~~~~~~~~~~~~~~~~~~~~~~~~
    /// <summary>
    /// Damit der Gegner auch verfolgt wird, wenn er sich im Verfolger-Range befindet, nachdem der Angriff erfolgt ist.
    /// </summary>
    /// <param name="collision"></param>
    private void CheckForPlayer()
    {
        // Alle Gegner detektieren:
        Collider2D[] hits = GetDetectedEnemies();

        //**************** Gegner gefunden ****************
        if (hits.Length > 0)
        {
            this.detectedEnemy = hits[0].transform;

            // Wenn Gegner hinter dem Pawn steht, weiter zum Checkpoint laufen
            if (CheckIfEnemyIsBehind())
            {
                ChangeState(SoldierState.SeeNoEnemy);
                return;
            }


            //-------------- Gegner angreifen ------------------
            // wenn sich ein Gegner in der Attack-Range befindet und der Cooldown abgelaufen ist 
            float enemyDistance = Vector2.Distance(this.transform.position, this.detectedEnemy.position);
            if (enemyDistance <= this.Config.MaxAttackRange)
            {
                if (this.attackCooldownTimer <= 0)
                {
                    // Angreifen:
                    this.attackCooldownTimer = this.Config.AttackCooldown;
                    ChangeState(SoldierState.Attack);
                    //TriggerAttackAnimation(enemyDirection);
                    // Nach den Angriff wird in der Animation wieder in den "IDle" Status gewechselt
                }
                else // if(this.State == SoldierState.SeeEnemy)
                {
                    // Vor Gegner stehen bleiben, wenn er sich in der Attack-Range befindet:
                    this.Rb.linearVelocity = Vector2.zero;
                    ChangeState(SoldierState.Idle);
                }

            }
            //-------------- Auf Gegner zulaufen ----------------
            else
            {
                ChangeState(SoldierState.SeeEnemy);
            }

        }
        //**************** keinen Gegner gefunden ****************
        else
        {
            ChangeState(SoldierState.SeeNoEnemy);
        }
    }


    //protected void TriggerAttackAnimation(Vector2 enemyDirection)
    //{
    //    Vector2 enemyDirection = (this.detectedEnemy.position - this.transform.position).normalized;
    //    if (enemyDirection.y > Mathf.Abs(enemyDirection.x) * 0.5f)
    //        animator.SetTrigger("AttackUp");
    //    else if (-enemyDirection.y > Mathf.Abs(enemyDirection.x) * 0.5f)
    //        animator.SetTrigger("AttackDown");
    //    else
    //        animator.SetTrigger("AttackStd");
    //}



    //~~~~~~~~~~~~~~~~~~~~~ Angreifen ~~~~~~~~~~~~~~~~~~~~~~~
    public void Attack()
    {
        // zum Angreifen stehen bleiben, Attacke wird durch den Zustandswechsel ausgelöst.
        // Die weitere Logik befindet sich in der Animation und in Enemy_Combat.cs
        //Debug.Log("Attacking player now");
        this.Rb.linearVelocity = Vector2.zero;
    }
    public void ChaseEnemy()
    {
        Move(this.detectedEnemy);
    }


    public void GoBackToTower()
    {
        this.homePoint?.GoBackToTower();
    }

}
