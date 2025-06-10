using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    //######################## Membervariablen ##############################
    protected Transform attackPoint;
    public ConfigTorch ConfigTorch { get; set; }

    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.attackPoint = transform.Find("AttackPoint");
        this.ConfigTorch = GetComponent<Torch>().ConfigTorch;
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.attackPoint.position, this.ConfigTorch.weaponRange, this.ConfigTorch.detectionLayer);

        // 1 Gegner Schaden zu fügen:
        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-this.ConfigTorch.damage);
            hits[0].GetComponent<Knockback>()?.KnockbackCharacter(this.transform,
                                                                    this.ConfigTorch.knockbackForce,
                                                                    this.ConfigTorch.knockbackTime,
                                                                    this.ConfigTorch.stunTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(this.attackPoint.position, this.ConfigTorch.weaponRange);
    }

}
