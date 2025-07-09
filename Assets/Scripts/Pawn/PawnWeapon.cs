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

    public Collider2D[] GetEnemiesInZone(Transform dynamiteExplosionPoint, float damageRadius)
    {
        return Physics2D.OverlapCircleAll(dynamiteExplosionPoint.position, damageRadius, this.config.DetectionLayer);
    }

    /// <summary>
    /// Fügt den Gegner Schaden zu
    /// </summary>
    public void AttackWeapon()
    {
        // Alle Objekte die in Waffen-Reichweite sind:
        Collider2D[] enemies = GetEnemiesInZone(this.attackPoint, this.config.WeaponRange);

        foreach (Collider2D enemy in enemies)
        {
            enemy.GetComponent<PlayerHealth>()?.ChangeHealth(-this.config.Damage);
            enemy.GetComponentInChildren<Health>()?.ChangeHealth(-this.config.Damage);
            if (this.config.EnableKnockbackShake)
            {
                enemy.GetComponent<Knockback>()?.KnockbackShake(this.transform,
                                                                this.config.KnockbackWidth,
                                                                this.config.KnockbackHeight,
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
