using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts;





public class Archer : MonoBehaviour
{
    //######################## Membervariablen ##############################
    // Pfeil + Bogen:
    public GameObject arrowPrefab;
    private Bow bow;

    // Gegner Detektion: 
    private Transform enemyDetectionPoint;             // Sichtradius-Mittelpunkt
    private Transform enemyTransform;                 // Transform-Attr. des detektierten Objektes

    private float attackTimer;
    public ConfigArcher ConfigArcher { get; set; }



    //########################### Geerbte Methoden #############################
    void Start()
    {
        this.bow = GetComponentInChildren<Bow>();
        this.enemyDetectionPoint = transform.Find("EnemyDetectionPoint");
        this.ConfigArcher = Resources.Load<ConfigArcher>("Config/Archer/Archer_Std");

        if (ConfigArcher == null)
            throw new Exception("Variable ConfigArcher = null");
        if(enemyDetectionPoint == null)
            throw new Exception("Variable enemyDetectionPoint = null");

        InitHealth(this.ConfigArcher);
    }

    void Update()
    {

        attackTimer -= Time.deltaTime;


        HandleEnemyDetection();

        if (this.bow.BowState == FireWeaponState.SeeEnemy && this.attackTimer <= 0) // Input.GetButtonDown("UserAttack")
        {
            Attack();
        }
        
    }


    //################################ Methoden ###################################

    private void InitHealth(ConfigArcher config)
    {
        // Health-Objekt initialisieren:
        Health health = GetComponent<Health>();
        if (health == null)
        {
            Debug.LogError("Health-Komponente nicht gefunden!");
            return;
        }
        health.Init(config.maxHealth);
    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~ Verhaltens Methoden ~~~~~~~~~~~~~~~~~~~~~~~~~~
    private void HandleEnemyDetection()
    {
        // Alle Gegner detektieren:
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.enemyDetectionPoint.position, this.ConfigArcher.playerDetectionRange, this.ConfigArcher.detectionLayer);

        if (hits.Length > 0)
        {
            this.bow.ChangeState(FireWeaponState.SeeEnemy);
            this.enemyTransform = hits[0].transform;
            
            // Player zum Gegner drehen + Bogen auf Gegner richten:
            Vector2 flipDirection = (this.enemyTransform.position - this.transform.position).normalized;
            FlipCharakterIfNecessary(flipDirection.x);
            this.bow.RotateBowToTarget(this.enemyTransform.position);
        }
        else
        {
            this.bow.ChangeState(FireWeaponState.SeeNoEnemy);
            this.enemyTransform = null;
        }
    }
     
    public void Attack()
    {
        this.bow.Attack_Enemy(this.enemyTransform);
        this.attackTimer = this.ConfigArcher.attackCooldown;
    }







    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Movement Methoden ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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



    //~~~~~~~~~~~~~~~~~~~ Gizmos Methoden ~~~~~~~~~~~~~~~~~~~~~~
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.enemyDetectionPoint.position, this.ConfigArcher.playerDetectionRange);
    }

}
