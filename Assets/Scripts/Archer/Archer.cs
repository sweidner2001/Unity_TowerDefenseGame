using System.Collections.Generic;
using UnityEngine;
using System;





public class Archer : MonoBehaviour
{
    //######################## Membervariablen ##############################
    

    // Unser Schuss-Objekt (Pfeil)
    public GameObject arrowPrefab;
    private Bow bow;



    // Gegner Detektion: 
    public Transform detectionPoint;            // Sichtradius-Mittelpunkt
    public LayerMask detectionLayer;            // was wollen wir detektieren?
    private Transform aimTransform;             // Transform-Attr. des detektierten Objektes


    private float attackTimer;



    // Level up: StatsManager + Sprites/Animation austauschen!
    public StatsManagerArcher smArcher;



    //########################### Geerbte Methoden #############################
    void Start()
    {
        this.bow = GetComponentInChildren<Bow>();
        this.smArcher = StatsManagerArcher.Instance;
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.detectionPoint.position, this.smArcher.playerDetectionRange, this.detectionLayer);

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
        this.attackTimer = this.smArcher.attackCooldown;
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
        Gizmos.DrawWireSphere(this.detectionPoint.position, this.smArcher.playerDetectionRange);
    }

}
