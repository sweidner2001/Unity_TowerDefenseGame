using System.Collections.Generic;
using UnityEngine;
using System;





public class Archer : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public Transform arrowLaunchPoint;

    // Unser Schuss-Objekt (Pfeil)
    public GameObject arrowPrefab;

    private Vector2 aimDirection = Vector2.right;


    // Gegner Detektion: 
    public float playerDetectionRange = 4f;
    public Transform detectionPoint;
    public LayerMask detectionLayer;            // was wollen wir detektieren?
    private Transform aimTransform;          // Transform-Attr. des detektierten Objektes


    public float attackCooldown = 2;
    private float attackTimer;


    private Animator animatorBow;
    private Animator animatorBody;
    private Dictionary<ArcherBowState, string> stateToAnimation = new Dictionary<ArcherBowState, string>()
    {
        { ArcherBowState.SeeNoEnemy, "isSeeNoEnemy" },
        { ArcherBowState.SeeEnemy, "isSeeEnemy" },
        { ArcherBowState.Attack, "isAttacking" }
    };



    private ArcherBowState _bowState;
    public ArcherBowState BowState
    {
        get => _bowState;
        set
        {
            // Alte Animation deaktivieren
            if (!stateToAnimation.ContainsKey(value))
                throw new Exception($"State {value} ist nicht vorhanden!");

            animatorBow.SetBool(stateToAnimation[_bowState], false);

            // Zustand aktualisieren
            _bowState = value;

            // Neue Animation aktivieren
            animatorBow.SetBool(stateToAnimation[_bowState], true);
        }
    }


    //########################### Geerbte Methoden #############################
    void Start()
    {
        this.animatorBow = GetComponent<Animator>();
        this.BowState = ArcherBowState.SeeNoEnemy;
        
    }

    void Update()
    {

        attackTimer -= Time.deltaTime;


        HandleAiming();

        if (this.BowState == ArcherBowState.SeeEnemy && this.attackTimer <= 0) // Input.GetButtonDown("UserAttack")
        {
            Attack();
        }
        
    }



    private void HandleAiming()
    {
        // Alle Gegner detektieren:
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.detectionPoint.position, this.playerDetectionRange, this.detectionLayer);

        if (hits.Length > 0)
        {
            this.BowState = ArcherBowState.SeeEnemy;
            this.aimTransform = hits[0].transform;
            this.aimDirection = (this.aimTransform.position - this.arrowLaunchPoint.position).normalized;
            Vector2 flipDirection = (this.aimTransform.position - this.transform.position).normalized;
            FlipCharakterIfNecessary(flipDirection.x);
        }
        else
        {
            this.BowState = ArcherBowState.SeeNoEnemy;
            this.aimTransform = null;
        }
    }
     
    public void Attack()
    {
        this.BowState = ArcherBowState.Attack;
        Arrow arrow = Instantiate(arrowPrefab, arrowLaunchPoint.position, Quaternion.identity).GetComponent<Arrow>();
        arrow.arrowDirection = aimDirection;
        this.attackTimer = this.attackCooldown;
    }





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




    public void ChangeState(ArcherBowState newState)
    {
        this.BowState = newState;
    }




    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.detectionPoint.position, this.playerDetectionRange);
    }

}
