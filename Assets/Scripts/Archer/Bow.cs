using Assets.Scripts;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.STP;




public class Bow : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public GameObject arrowPrefab;
    public Transform arrowLaunchPoint;
    public Vector2 aimDirection = Vector2.right;

    // Level up: StatsManager + Sprites/Animation austauschen!
    public ConfigArcher ConfigArcher;
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
        protected set
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
        this.ConfigArcher = transform.parent.GetComponent<Archer>().ConfigArcher;
        this.arrowConfig = Resources.Load<ConfigArrow>("Config/Archer/Arrow_Std");
    }

    void Update()
    {

    }



    //############################ Methoden ##############################
    public void ChangeState(FireWeaponState newState)
    {
        BowState = newState;

        if (newState == FireWeaponState.SeeNoEnemy)
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
    private ConfigArrow arrowConfig;
    public Arrow CreateArrow()
    {
        Arrow arrow = Instantiate(arrowPrefab, arrowLaunchPoint.position, Quaternion.identity).GetComponent<Arrow>();
        arrow.Init(this.arrowConfig, this.enemyTransform, HandleArrowCollision, this.ConfigArcher.detectionLayer);
        return arrow;
    }

    private void HandleArrowCollision(Collision2D collision)
    {

        collision.gameObject.GetComponentInChildren<PlayerHealth>()?.ChangeHealth(-this.ConfigArcher.damage);

        if (this.ConfigArcher.knockbackEnabled)
        {
            collision.gameObject.GetComponentInChildren<Knockback>()?.KnockbackCharacter(this.transform,
                                                                                         this.ConfigArcher.knockbackForce,
                                                                                         this.ConfigArcher.knockbackTime,
                                                                                         this.ConfigArcher.stunTime);
        }
    }



    //---------------- Ziel anvisieren ------------------
    public void RotateBowToTarget(Vector3 targetPosition)
    {
        // Winkel berechnen:
        //Vector2 direction = (targetPosition - transform.position).normalized;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float angle = Arrow.GetBowRotationAngle(this.transform.position, targetPosition, this.arrowConfig);


        // Prüfe, ob das Parent-Objekt gespiegelt ist
        bool isFlipped = transform.parent != null && transform.parent.localScale.x < 0;
        if (isFlipped)
        {
            // Wenn Archer gespiegelt, dann invertiere den Winkel.
            // 50 wird zu -50 im Inpsector, weil die Z-Achse invertiert wird, 
            // weswegen 180 - angle nicht funktioniert!
            angle = angle - 180;
        }

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}