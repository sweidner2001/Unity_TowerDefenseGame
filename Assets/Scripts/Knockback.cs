using System;
using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    //######################## Membervariablen ##############################
    private Rigidbody2D rb;
    private Enemy_Movement2 enemyMovement;
    private PlayerMovement playerMovement;
    public bool isScriptOnPlayer = false;

    private Action knockbackStateAction;
    private Action afterknockbackStateAction;


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.rb = GetComponentInParent<Rigidbody2D>();

        if (isScriptOnPlayer)
        {
            this.playerMovement = GetComponent<PlayerMovement>();
            this.knockbackStateAction = PlayerKnockbackState_Handler;
            this.afterknockbackStateAction = PlayerAfterKnockbackState_Handler;
        } else
        {
            this.enemyMovement = GetComponent<Enemy_Movement2>();
            this.knockbackStateAction = StdKnockbackState_Handler;
            this.afterknockbackStateAction = StdAfterKnockbackState_Handler;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //########################### Event-Handler #############################
    private void StdKnockbackState_Handler()
    {
        this.enemyMovement.ChangeState(EnemyState.Knockback);
    }

    private void StdAfterKnockbackState_Handler()
    {
        this.enemyMovement.ChangeState(EnemyState.Idle);
    }


    private void PlayerKnockbackState_Handler()
    {
        this.playerMovement.isKnockedBAck = true;
    }

    private void PlayerAfterKnockbackState_Handler()
    {
        this.playerMovement.isKnockedBAck = false;
    }


    //########################### Methoden ############################
    public void KnockbackCharacter(Transform forceTransform, float knockbackForce, float knockbackTime, float stunTime)
    {
        if (!this.gameObject.activeInHierarchy)
            return;

        //this.enemyMovement.ChangeState(EnemyState.Knockback);
        this.knockbackStateAction();

        // Ich werde zurückgestoßen
        Vector2 direction = (this.transform.position - forceTransform.position).normalized;
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

        // Kontrolle zurückgeben / wie lange bleibt Gegner noch stehen?
        yield return new WaitForSeconds(stunTime);
        this.afterknockbackStateAction();
        //enemyMovement.ChangeState(EnemyState.Idle);
    }
}
