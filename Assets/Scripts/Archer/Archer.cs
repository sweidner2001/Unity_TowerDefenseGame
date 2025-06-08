using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts;





public class Archer : MonoBehaviour
{
    //######################## Membervariablen ##############################
    // Unser Schuss-Objekt (Pfeil)
    public GameObject arrowPrefab;
    private Bow bow;
    private float attackTimer;


    // Gegner Detektion: 
    public Transform enemyDetectionPoint;           // Sichtradius-Mittelpunkt
    private Transform aimTransform;                 // Transform-Attr. des detektierten Objektes

    // Level up: StatsManager + Sprites/Animation austauschen!
    public ConfigArcher ConfigArcher { get; set; }



    //########################### Geerbte Methoden #############################
    void Start()
    {
        this.bow = GetComponentInChildren<Bow>();
        this.ConfigArcher = Resources.Load<ConfigArcher>("Config/Archer/Archer_Std");

        if(ConfigArcher == null)
            Debug.Log("Config archer ist null!");
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

    //~~~~~~~~~~~~~~~~~~~~~~~~~ Verhaltens Methoden ~~~~~~~~~~~~~~~~~~~~~~~~~~
    private void HandleEnemyDetection()
    {
        // Alle Gegner detektieren:
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.enemyDetectionPoint.position, this.ConfigArcher.playerDetectionRange, this.ConfigArcher.detectionLayer);

        if (hits.Length > 0)
        {
            this.bow.ChangeState(FireWeaponState.SeeEnemy);
            this.aimTransform = hits[0].transform;
            

            Vector2 flipDirection = (this.aimTransform.position - this.transform.position).normalized;
            FlipCharakterIfNecessary(flipDirection.x);
            this.bow.RotateBowToTarget(this.aimTransform.position);
        }
        else
        {
            this.bow.ChangeState(FireWeaponState.SeeNoEnemy);
            this.aimTransform = null;
        }
    }
     
    public void Attack()
    {
        this.bow.Attack_Enemy(this.aimTransform);
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
