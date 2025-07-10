using Assets.Scripts;
using System;
using UnityEngine;

public class TNTMan : SoldierBase<ConfigTNTMan>
{
    //######################## Membervariablen ##############################
    protected HomePoint homePoint;
    protected TNTManWeapon weapon;



    //########################### Geerbte Methoden #############################
    protected override void Start()
    {

        base.Start();
        this.Config = GetConfig();
        this.weapon = GetComponent<TNTManWeapon>();

        try
        {
            if (Config == null)
                throw new Exception("Variable ConfigTorch = null");
            if (enemyDetectionPoint == null)
                throw new Exception("Variable enemyDetectionPoint = null");

            InitHealth(this.Config.MaxHealth);
            this.homePoint = GetComponent<HomePoint>();
            this.homePoint?.Init();
            ChangeState(SoldierState.SeeNoEnemy);

        }
        catch (Exception e)
        {
            Debug.LogWarning(e.ToString());
        }
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
    public override ConfigTNTMan GetConfig()
    {
        if (Config == null)
        {
            string pfad = "Config/TNTMan/TNTMan_Std";
            Config = Resources.Load<ConfigTNTMan>(pfad);

            if (Config == null)
            {
                throw new Exception($"Config {pfad} konnte nicht geladen werden!");
            }
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

                // Zum Gegner drehen:
                Vector2 flipDirection = (this.detectedEnemy.position - this.transform.position).normalized;
                FlipCharakterIfNecessary(flipDirection.x);

                if (this.attackCooldownTimer <= 0)
                {
                    // Angreifen:
                    this.attackCooldownTimer = this.Config.AttackCooldown;
                    ChangeState(SoldierState.Attack);
                    // Nach den Angriff wird in der Animation wieder in den "IDle" Status gewechselt
                }
                else if (this.State == SoldierState.SeeEnemy)
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



    //~~~~~~~~~~~~~~~~~~~~~ Angreifen ~~~~~~~~~~~~~~~~~~~~~~~
    public void Attack()
    {
        // zum Angreifen stehen bleiben, Attacke wird durch den Zustandswechsel ausgelöst.
        // Die weitere Logik befindet sich in der Animation und in Enemy_Combat.cs
        //Debug.Log("Attacking player now");
        this.Rb.linearVelocity = Vector2.zero;
        this.weapon.AttackEnemy(this.detectedEnemy);
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
