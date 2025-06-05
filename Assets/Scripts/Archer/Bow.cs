using System.Collections.Generic;
using UnityEngine;
using System;


public enum ArcherBowState : int
{
    SeeNoEnemy,
    SeeEnemy,
    Attack,
}

public class Bow : MonoBehaviour
{
    //######################## Membervariablen ##############################

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
            // Alte Animation deaktivieren
            if (!stateToAnimation.ContainsKey(value))
                throw new Exception($"State {value} ist nicht vorhanden!");

            animator.SetBool(stateToAnimation[_bowState], false);

            // Zustand aktualisieren
            _bowState = value;

            // Neue Animation aktivieren
            animator.SetBool(stateToAnimation[_bowState], true);
        }
    }




    public void Attack()
    {
        //this.BowState = ArcherBowState.Attack;
        //Arrow arrow = Instantiate(arrowPrefab, arrowLaunchPoint.position, Quaternion.identity).GetComponent<Arrow>();
        //arrow.arrowDirection = aimDirection;
        //this.attackTimer = this.attackCooldown;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
