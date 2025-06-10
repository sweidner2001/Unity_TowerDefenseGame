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
        this.Config = Resources.Load<ConfigTorch>("Config/Torch/Torch_Std");

        try
        {
            if (Config == null)
                throw new Exception("Variable ConfigTorch = null");
            if (enemyDetectionPoint == null)
                throw new Exception("Variable enemyDetectionPoint = null");

            InitHealth(this.Config.MaxHealth);
            this.homePoint = GetComponent<HomePoint>();
            this.homePoint?.Init();
            //InitHomePoint();
            ChangeState(SoldierState.SeeNoEnemy);
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
    //public void ChangeState(SoldierState newState)
    //{
    //    this.State = newState;
    //}



    /// <summary>
    /// Damit der Gegner auch verfolgt wird, wenn er sich im Verfolger-Range befindet, nachdem der Angriff erfolgt ist.
    /// </summary>
    /// <param name="collision"></param>
    private void CheckForPlayer()
    {
        // Alle Gegner detektieren:
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.enemyDetectionPoint.position,
                                                        this.Config.PlayerDetectionRange,
                                                        this.Config.DetectionLayer);

        //**************** Gegner gefunden ****************
        if (hits.Length > 0)
        {

            this.detectedEnemy = hits[0].transform;

            //-------------- Gegner angreifen ------------------
            // wenn sich ein Gegner in der Attack-Range befindet und der Cooldown abgelaufen ist 
            float enemyDistance = Vector2.Distance(this.transform.position, this.detectedEnemy.position);
            if (enemyDistance <= this.Config.MaxAttackRange && this.attackCooldownTimer <= 0)
            {
                // Angreifen:
                this.attackCooldownTimer = this.Config.AttackCooldown;
                ChangeState(SoldierState.Attack);

                // Nach den Angriff wird in der Animation wieder in den "IDle" Status gewechselt
            }
            if (enemyDistance <= this.Config.MaxAttackRange && this.State == SoldierState.SeeEnemy)
            {
                // Vor Gegner stehen bleiben, wenn er sich in der Attack-Range befindet:
                this.rb.linearVelocity = Vector2.zero;
                ChangeState(SoldierState.Idle);
                Debug.Log("Gegner gefunden - #Stehen bleiben");
            }
            //-------------- Auf Gegner zulaufen ----------------
            // eine begonenne Attacke soll zuerst zu Ende laufen
            else if (enemyDistance > this.Config.MaxAttackRange && this.State != SoldierState.Attack)
            {
                ChangeState(SoldierState.SeeEnemy);
                //Debug.Log("Gegner gefunden - hinlaufen");
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


    public void GoBackToTower()
    {
        this.homePoint?.GoBackToTower();
    }


    //~~~~~~~~~~~~~~~~~~~~~~~ Movement ~~~~~~~~~~~~~~~~~~~~~~~~~~

    //public void Move(Transform destinationTransform)
    //{
    //    // Richtungsvektor
    //    Vector2 direction = (destinationTransform.position - this.transform.position).normalized;
    //    this.rb.linearVelocity = direction * this.ConfigTorch.movingSpeed;

    //    // aktuelle Bewegung abrufen
    //    FlipCharakterIfNecessary(this.rb.linearVelocity.x);
    //}


    ///// <summary>
    ///// Dreht das Sprite Bild um 180°, wenn die Figur beim Gehen die Richtung wechselt
    ///// </summary>
    ///// <param name="horizontalMovement">relative Bewegungsrichtung in X-Achse von der Figur aus gesehen</param>
    //protected void FlipCharakterIfNecessary(float horizontalMovement)
    //{
    //    // horizontal > 0 --> nach rechts laufen, aber Bild links ausgerichtet
    //    // horizontal < 0 --> nach links laufen, aber Bild rechts ausgerichtet
    //    if (horizontalMovement > 0 && this.transform.localScale.x < 0 ||
    //        horizontalMovement < 0 && this.transform.localScale.x > 0)
    //    {
    //        Flip();
    //    }
    //}


    ///// <summary>
    ///// Dreht das Sprite Bild um 180°
    ///// </summary>
    //protected void Flip()
    //{
    //    this.transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    //}





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