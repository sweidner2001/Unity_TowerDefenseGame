using System;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.UIElements;


public enum FireWeaponState : int
{
    SeeNoEnemy,
    SeeEnemy,
    Attack,
}

public class Bow : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public GameObject arrowPrefab;
    public Transform arrowLaunchPoint;
    public Vector2 aimDirection = Vector2.right;

    // Level up: StatsManager + Sprites/Animation austauschen!
    public StatsManagerArcher smArcher;



    // Animation:
    private Animator animator;
    private Dictionary<FireWeaponState, string> stateToAnimation = new Dictionary<FireWeaponState, string>()
    {
        { FireWeaponState.SeeNoEnemy, "isSeeNoEnemy" },
        { FireWeaponState.SeeEnemy, "isSeeEnemy" },
        { FireWeaponState.Attack, "isAttacking" }
    };

    private FireWeaponState _bowState;
    public FireWeaponState BowState
    {
        get => _bowState;
        set
        {
            // Fehlerprüfung:
            if (!stateToAnimation.ContainsKey(value))
                throw new Exception($"State {value} ist nicht vorhanden!");

            // Alte Animation deaktivieren + neue aktivieren:
            animator.SetBool(stateToAnimation[_bowState], false);
            _bowState = value;
            animator.SetBool(stateToAnimation[value], true);
        }
    }





    //############################ Geerbte Methoden ##############################
    void Start()
    {
        animator = GetComponent<Animator>();
        BowState = FireWeaponState.SeeNoEnemy;
        this.smArcher = StatsManagerArcher.Instance;
        this.arrowConfig= Resources.Load<ArrowConfig>("Arrow_Std");
    }

    void Update()
    {
        
    }



    //############################ Methoden ##############################
    public void ChangeState(FireWeaponState newState)
    {
        BowState = newState;
    }



    //--------------- Attacke ------------------
    // Attacke wird von Archer ausgelöst
    public void Attack(Vector3 enemyPosition)
    {
        BowState = FireWeaponState.Attack;
        this.aimDirection = (enemyPosition - this.arrowLaunchPoint.position).normalized;
        
    }
    Transform enemyTransform;

    public void Attack_Enemy(Transform enemyTransform)
    {
        BowState = FireWeaponState.Attack;
        this.aimDirection = (enemyTransform.position - this.arrowLaunchPoint.position).normalized;
        this.enemyTransform = enemyTransform;
    }


    public void Shoot()
    {
        CreateArrow();
    }



    //-------------- Projektil / Pfeil ------------------
    private ArrowConfig arrowConfig;
    public Arrow CreateArrow()
    {
        Arrow arrow = Instantiate(arrowPrefab, arrowLaunchPoint.position, Quaternion.identity).GetComponent<Arrow>();
        arrow.ArrowDirection = this.aimDirection;
        //arrow.ArrowSpeed = this.smArcher.arrowSpeed;
        arrow.MaxFlightDistance = this.smArcher.playerDetectionRange;
        if (arrowConfig == null)
            Debug.Log($"ArrowConfig ist null");
        arrow.Config = arrowConfig;
        //arrow.lifeSpanOnHittedObject = this.smArcher.arrowLifeSpanOnHittetObject;
        arrow.OnEnemyArrowCollision += HandleArrowCollision;
        arrow.enemyTransform = this.enemyTransform;
        return arrow;
    }

    private void HandleArrowCollision(Collision2D collision)
    {
        
        collision.gameObject.GetComponent<PlayerHealth>()?.ChangeHealth(-this.smArcher.damage);
        // TODO: Knockbacktime fehlt!!
        collision.gameObject.GetComponent<PlayerMovement>()?.Knockback(forceTransform: this.transform, this.smArcher.knockbackForce, this.smArcher.stunTime);
    }



    //---------------- Ziel anvisieren ------------------
    public void RotateBowToTarget(Vector3 targetPosition)
    {
        // Winkel berechnen:
        //Vector2 direction = (targetPosition - transform.position).normalized;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float angle = Arrow.GetBowRotationAngle(this.transform.position, targetPosition, this.arrowConfig, this.smArcher.playerDetectionRange);


        // Prüfe, ob das Parent-Objekt gespiegelt ist
        bool isFlipped = transform.parent != null && transform.parent.localScale.x < 0;
        if (isFlipped)
        {
            // Wenn Archer gespiegelt, dann invertiere den Winkel.
            // 50° wird zu -50° im Inpsector, weil die Z-Achse invertiert wird, 
            // weswegen 180 - angle nicht funktioniert!
            angle = angle - 180;
        }

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
