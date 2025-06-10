using Assets.Scripts;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;


public enum AttackDirection
{
    Up,
    Down,
    Standard
}

public class SoldierTorch_Weapon : MonoBehaviour
{
    //######################## Membervariablen ##############################
    protected Transform attackPoint;
    protected Transform attackPointStd;
    protected Transform attackPointUp;
    protected Transform attackPointDown;
    public ConfigTorch Config { get; set; }






    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.attackPointStd = transform.Find("AttackPoint");
        this.attackPointUp = transform.Find("AttackPointUp");
        this.attackPointDown = transform.Find("AttackPointDown");

        if(this.attackPointStd == null || this.attackPointUp == null || this.attackPointDown == null)
        {
            Debug.LogError("Attack points not set up correctly. Please ensure AttackPoint, AttackPointUp, and AttackPointDown are assigned in the GameObject hierarchy.");
        }

        SetAttackDirection(AttackDirection.Standard);
        this.Config = GetComponent<SoldierTorch>().Config;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private AttackDirection attackDirection;
    public void SetAttackDirection(AttackDirection attackDirection)
    {
        this.attackDirection = attackDirection;

        switch (attackDirection)
        {
            case AttackDirection.Up:
                this.attackPoint = this.attackPointUp;
                Debug.Log("Attack Up:");
                break;
            case AttackDirection.Down:
                this.attackPoint = this.attackPointDown;
                Debug.Log("Attack Down:");
                break;
            case AttackDirection.Standard:
            default:
                this.attackPoint = this.attackPointStd;
                Debug.Log("Attack std:");
                break;
        }
    }



    //########################### Methoden #############################
    /// <summary>
    /// Wird aufgerufen, wenn die Figur mit einem anderen Collider kollidiert
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }


    /// <summary>
    /// Fügt den Gegner Schaden zu
    /// </summary>
    public void Attack()
    {
        //Collider2D[] hits;


        //switch (attackDirection)
        //{
        //    case AttackDirection.Up:
        //        hits = Physics2D.OverlapCircleAll(this.attackPointUp.position, this.Config.WeaponRange, this.Config.DetectionLayer);
        //        this.attackPoint = this.attackPointUp;
        //        Debug.Log("Attack Up:" + this.attackPointUp.position);
        //        break;
        //    case AttackDirection.Down:
        //        hits = Physics2D.OverlapCircleAll(this.attackPointDown.position, this.Config.WeaponRange, this.Config.DetectionLayer);
        //        this.attackPoint = this.attackPointDown;
        //        Debug.Log("Attack Down:" + this.attackPointDown.position);
        //        break;
        //    case AttackDirection.Standard:
        //    default:
        //        hits = Physics2D.OverlapCircleAll(this.attackPointStd.position, this.Config.WeaponRange, this.Config.DetectionLayer);
        //        this.attackPoint = this.attackPointStd;
        //        Debug.Log("Attack std:" + this.attackPointStd.position);
        //        break;
        //}

        // Alle Objekte die in Waffen-Reichweite sind:
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.attackPoint.position, this.Config.WeaponRange, this.Config.DetectionLayer);

        // 1 Gegner Schaden zu fügen:
        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-this.Config.Damage);
            if (this.Config.KnockbackEnabled)
            {
                hits[0].GetComponent<Knockback>()?.KnockbackCharacter(this.transform,
                                                                    this.Config.KnockbackForce,
                                                                    this.Config.KnockbackTime,
                                                                    this.Config.StunTime);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(this.attackPoint.position, this.Config.WeaponRange);
    }

}
