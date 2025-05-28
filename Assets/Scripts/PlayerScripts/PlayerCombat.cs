using System.Threading;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    //######################## Membervariablen ##############################
    //public int damage = 1;
    private Animator anim;


    //public float attackRange = 0.7f;
    //public float attackCooldown = 2;
    private float attackCooldownTimer;


    public Transform attackPoint;
    //public float weaponRange;
    public LayerMask enemyLayer;       // Wen wollen wir Schaden zu fügen?

    //public float knockbackForce = 3;
    //public float stunTime = 0.2f;
    //public float knockbackTime = 0.15f;


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.attackCooldownTimer > 0)
        {
            this.attackCooldownTimer -= Time.deltaTime;
        }
    }



    //########################### Methoden #############################
    public void Attack()
    {
        if(this.attackCooldownTimer <= 0)
        {
            anim.SetBool("isAttacking", true);
            this.attackCooldownTimer = PlayerStatsManager.Instance.attackCooldown;
        }
    }

    public void DealDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(this.attackPoint.position, PlayerStatsManager.Instance.weaponRange, this.enemyLayer);
        
        // 1 Gegner Schaden zu fügen:
        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<EnemyHealth>().ChangeHealth(-PlayerStatsManager.Instance.damage);
            enemies[0].GetComponent<EnemyKnockback>()?.Knockback(playerTransform: this.transform, PlayerStatsManager.Instance.knockbackForce, PlayerStatsManager.Instance.knockbackTime, PlayerStatsManager.Instance.stunTime);
        }
    }

    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
    }


    /// <summary>
    /// Wird aufgerufen, wenn der Charakter mit einen anderen Collider kollidiert
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.tag == "Enemy")
        //{
        //    collision.gameObject.GetComponent<PlayerHealth>()?.ChangeHealth(-damage);
        //}
    }



    /// <summary>
    /// Zeichnet den Detection Point mit Radius für Gegnerische Figuren
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.attackPoint.position, PlayerStatsManager.Instance.weaponRange);
    }

}
