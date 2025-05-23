using System.Collections;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    //######################## Membervariablen ##############################
    private Rigidbody2D rb;
    private Enemy_Movement2 enemyMovement;





    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.enemyMovement = GetComponent<Enemy_Movement2>();
        //this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    //########################### Methoden ############################
    public void Knockback(Transform playerTransform, float knockbackForce, float knockbackTime, float stunTime)
    {
        this.enemyMovement.ChangeState(EnemyState.Knockback);
        //isKnockedBAck = true;
        Vector2 direction = (this.transform.position - playerTransform.position).normalized;
        this.rb.linearVelocity = direction * knockbackForce;
        StartCoroutine(StunTimer(knockbackTime, stunTime));
    }



    //########################### Coroutinen #############################
    private IEnumerator StunTimer(float knockbackTime, float stunTime)
    {
        // Wartezeit
        yield return new WaitForSeconds(knockbackTime);

        // anschließend Figur zum stehen bringen und die Bewegungs-Kontrolle zurückgeben
        this.rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        enemyMovement.ChangeState(EnemyState.Idle);
    }
}
