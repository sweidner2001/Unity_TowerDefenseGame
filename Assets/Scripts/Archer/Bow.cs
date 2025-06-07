using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.STP;


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
    private Transform enemyTransform;



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
            // Fehlerpr�fung:
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
        ChangeState(FireWeaponState.SeeNoEnemy);
        this.smArcher = StatsManagerArcher.Instance;
        this.arrowConfig= Resources.Load<ArrowConfig>("Config/Arrow_Std");
    }

    void Update()
    {
        
    }



    //############################ Methoden ##############################
    public void ChangeState(FireWeaponState newState)
    {
        BowState = newState;

        if(newState == FireWeaponState.SeeNoEnemy)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }



    //--------------- Attacke ------------------
    // Attacke wird von Archer ausgel�st
    public void Attack(Vector3 enemyPosition)
    {
        ChangeState(FireWeaponState.Attack);
        this.aimDirection = (enemyPosition - this.arrowLaunchPoint.position).normalized;
        
    }
    

    public void Attack_Enemy(Transform enemyTransform)
    {
        ChangeState(FireWeaponState.Attack);
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
        arrow.Init(this.arrowConfig, this.enemyTransform, HandleArrowCollision, this.smArcher.detectionLayer);
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

        float angle = Arrow.GetBowRotationAngle(this.transform.position, targetPosition, this.arrowConfig);


        // Pr�fe, ob das Parent-Objekt gespiegelt ist
        bool isFlipped = transform.parent != null && transform.parent.localScale.x < 0;
        if (isFlipped)
        {
            // Wenn Archer gespiegelt, dann invertiere den Winkel.
            // 50� wird zu -50� im Inpsector, weil die Z-Achse invertiert wird, 
            // weswegen 180 - angle nicht funktioniert!
            angle = angle - 180;
        }

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
