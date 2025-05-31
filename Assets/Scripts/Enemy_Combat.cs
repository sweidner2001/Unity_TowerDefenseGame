using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public int damage = 1;
    public Transform attackPoint;
    public float weaponRange = 1;
    public LayerMask playerLayer;       // Wen wollen wir Schaden zu fügen?
    public float knockbackForce = 3;
    public float stunTime = 0.2f;              // Zeit wie lange der Gegner nach Attacke bewegungsumfähig ist


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);

        // 1 Gegner Schaden zu fügen:
        if (hits.Length > 0)
        {
            hits[0].GetComponent<PlayerHealth>().ChangeHealth(-damage);
            hits[0].GetComponent<PlayerMovement>().Knockback(enemyTransform: this.transform, knockbackForce, stunTime);
        }
    }

}
