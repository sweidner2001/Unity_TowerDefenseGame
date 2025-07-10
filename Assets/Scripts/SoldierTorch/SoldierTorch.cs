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





public class SoldierTorch : SoldierBase<ConfigTorch>
{

    //######################## Membervariablen ##############################
    protected HomePoint homePoint;




    //########################### Geerbte Methoden #############################
    protected override void Start()
    {

        base.Start();
        this.Config = GetConfig();//Resources.Load<ConfigTorch>("Config/Torch/Torch_Std");
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
        if (this.State == SoldierState.Knockback || this.State == SoldierState.Attack || this.State == SoldierState.Dead)
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
    public override ConfigTorch GetConfig()
    {
        if (Config == null)
        {
            Config = Resources.Load<ConfigTorch>("Config/Torch/Torch_Std");
        }
        return Config;
    }


    protected void InitHealth(int maxHealth, int coinsOnDeath=0)
    {
        // Health-Objekt initialisieren:
        Health health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("Health-Komponente nicht gefunden!");
            return;
        }
        health.Init(maxHealth, coinsOnDeath);
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
            Vector2 enemyDirection = (this.detectedEnemy.position - this.transform.position).normalized;

            //-------------- Gegner angreifen ------------------
            // wenn sich ein Gegner in der Attack-Range befindet und der Cooldown abgelaufen ist 
            float enemyDistance = Vector2.Distance(this.transform.position, this.detectedEnemy.position);
            if (enemyDistance <= this.Config.MaxAttackRange)
            {
                if(this.attackCooldownTimer <= 0)
                {
                    // Angreifen:
                    this.attackCooldownTimer = this.Config.AttackCooldown;
                    ChangeState(SoldierState.Attack);
                    TriggerAttackAnimation(enemyDirection);
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
            if (this.homePoint != null && this.State != SoldierState.OnTower)
            {
                ChangeState(SoldierState.SeeNoEnemy);
            }
            else if (this.State != SoldierState.OnTower)
            {
                // Stehen bleiben, kein Gegner gefunden und kein HomePoint zugewiesen
                this.Rb.linearVelocity = Vector2.zero;
                ChangeState(SoldierState.Idle);
            }
        }
    }


    protected void TriggerAttackAnimation(Vector2 enemyDirection)
    {
        if (enemyDirection.y > Mathf.Abs(enemyDirection.x) * 0.5f)
            animator.SetTrigger("AttackUp");
        else if (-enemyDirection.y > Mathf.Abs(enemyDirection.x) * 0.5f)
            animator.SetTrigger("AttackDown");
        else
            animator.SetTrigger("AttackStd");
    }



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