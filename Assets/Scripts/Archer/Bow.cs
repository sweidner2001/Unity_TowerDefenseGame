using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public enum ArcherBowState : int
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



    // Animation:
    private Animator animator;
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
        BowState = ArcherBowState.SeeNoEnemy;
    }

    void Update()
    {
        
    }



    //############################ Methoden ##############################
    public void ChangeState(ArcherBowState newState)
    {
        BowState = newState;
    }



    //--------------- Attacke ------------------
    // Attacke wird von Archer ausgelöst
    public void Attack(Vector3 enemyPosition)
    {
        BowState = ArcherBowState.Attack;
        this.aimDirection = (enemyPosition - this.arrowLaunchPoint.position).normalized;
    }


    public void Attack_Enemy(Transform enemyTransform)
    {
        BowState = ArcherBowState.Attack;
        this.aimDirection = (enemyTransform.position - this.arrowLaunchPoint.position).normalized;
    }


    public void Shoot()
    {
        Arrow arrow = Instantiate(arrowPrefab, arrowLaunchPoint.position, Quaternion.identity).GetComponent<Arrow>();
        arrow.arrowDirection = aimDirection;
    }




    //---------------- Ziel anvisieren ------------------
    public void RotateBowToTarget(Vector3 targetPosition)
    {
        // Winkel berechnen:
        Vector2 direction = (targetPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


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
