using UnityEngine;

public class SoldierTorch_Weapon : MonoBehaviour
{
    //######################## Membervariablen ##############################
    protected Transform attackPoint;
    public ConfigTorch Config { get; set; }

    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.attackPoint = transform.Find("AttackPoint");
        this.Config = GetComponent<SoldierTorch>().ConfigTorch;
    }

    // Update is called once per frame
    void Update()
    {

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
        // Alle Objekte die in Waffen-Reichweite sind:
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.attackPoint.position, this.Config.weaponRange, this.Config.detectionLayer);

        // 1 Gegner Schaden zu fügen:
        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-this.Config.damage);
            if (this.Config.knockbackEnabled)
            {
                hits[0].GetComponent<Knockback>()?.KnockbackCharacter(this.transform,
                                                                    this.Config.knockbackForce,
                                                                    this.Config.knockbackTime,
                                                                    this.Config.stunTime);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(this.attackPoint.position, this.Config.weaponRange);
    }

}
