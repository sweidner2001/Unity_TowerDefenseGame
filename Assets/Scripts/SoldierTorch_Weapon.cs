using Assets.Scripts;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;




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
        this.Config = GetComponent<SoldierTorch>().GetConfig();
        if (this.Config == null)
        {
            Debug.LogError("ConfigTorch is not set. Please assign a ConfigTorch in the Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAttackDirection(AttackDirection attackDirection)
    {

        switch (attackDirection)
        {
            case AttackDirection.Up:
                this.attackPoint = this.attackPointUp;
                break;
            case AttackDirection.Down:
                this.attackPoint = this.attackPointDown;
                break;
            case AttackDirection.Standard:
            default:
                this.attackPoint = this.attackPointStd;
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
    /// F�gt den Gegner Schaden zu
    /// </summary>
    public void Attack()
    {
        // Alle Objekte die in Waffen-Reichweite sind:
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.attackPoint.position, this.Config.WeaponRange, this.Config.DetectionLayer);

        // 1 Gegner Schaden zu f�gen:
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
