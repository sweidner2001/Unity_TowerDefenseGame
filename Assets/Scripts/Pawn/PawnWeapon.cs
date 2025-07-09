using Assets.Scripts;
using UnityEngine;

public class PawnWeapon : MonoBehaviour
{
    //######################## Membervariablen ##############################
    protected Transform attackPoint;
    protected Transform attackPointStd;
    protected Transform attackPointDown;
    protected ConfigPawn config;






    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.attackPointStd = transform.Find("AttackPoint");
        this.attackPointDown = transform.Find("AttackPointDown");

        if (this.attackPointStd == null || this.attackPointDown == null)
        {
            Debug.LogError("Attack points not set up correctly. Please ensure AttackPoint, AttackPointUp, and AttackPointDown are assigned in the GameObject hierarchy.");
        }

        SetAttackDirection(AttackDirection.Standard);
        this.config = GetComponent<Pawn>().GetConfig();
        if (this.config == null)
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
    /// Fügt den Gegner Schaden zu
    /// </summary>
    public void AttackWeapon()
    {
        // Alle Objekte die in Waffen-Reichweite sind:
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.attackPoint.position, this.config.WeaponRange, this.config.DetectionLayer);

        // 1 Gegner Schaden zu fügen:
        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>()?.ChangeHealth(-this.config.Damage);
            hits[0].GetComponentInChildren<Health>()?.ChangeHealth(-this.config.Damage);
            if (this.config.KnockbackEnabled)
            {
                hits[0].GetComponent<Knockback>()?.KnockbackCharacter(this.transform,
                                                                    this.config.KnockbackForce,
                                                                    this.config.KnockbackTime,
                                                                    this.config.StunTime);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(this.attackPoint.position, this.config.WeaponRange);
    }
}
