using Assets.Scripts;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using static UnityEngine.Rendering.STP;

public class Knockback : MonoBehaviour
{
    //######################## Membervariablen ##############################
    private Rigidbody2D rb;
    private ISoldierBase soldierBase;
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
        }
        else
        {
            this.soldierBase = GetComponent<ISoldierBase>();
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
        this.soldierBase.ChangeState(SoldierState.Knockback);
    }

    private void StdAfterKnockbackState_Handler()
    {
        this.soldierBase.ChangeState(SoldierState.Idle);
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
    public void KnockbackClassic(Transform forceSource, float knockbackForce, float knockbackTime, float stunTime)
    {
        if (!this.gameObject.activeInHierarchy)
            return;

        this.knockbackStateAction();

        // Ich werde zurückgestoßen
        Vector2 direction = (this.transform.position - forceSource.position).normalized;
        this.rb.linearVelocity = direction * knockbackForce;
        StartCoroutine(StunTimer(knockbackTime, stunTime));
    }

    public void KnockbackShake(Transform forceSource, float jumpWidth, float jumpHeight, float knockbackTime, float stunTime)
    {
        if (!this.gameObject.activeInHierarchy)
            return;

        this.knockbackStateAction();
        StartCoroutine(ShakeKnockbackCoroutine(forceSource, jumpHeight, jumpWidth, knockbackTime, stunTime));
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
    }



    private static Vector2 GetP1(Vector2 P0, Vector2 P2, float height)
    {
        Vector2 midPoint = (P0 + P2) * 0.5f;
        Vector2 P1 = midPoint + Vector2.up * height;
        return P1;
    }

    private static Vector2 GetP2(Vector2 P0, Transform forceTransform, float width)
    {
        // Bestimme, ob forceTransform links oder rechts von rb.position ist
        bool isForceFromLeft = forceTransform.position.x < P0.x;
        Vector2 forceDirection = isForceFromLeft ? Vector2.right : Vector2.left;

        Vector2 P2 = P0 + forceDirection * width;
        return P2;
    }

    private IEnumerator ShakeKnockbackCoroutine(Transform forceTransform, float jumpHeight, float jumpWidth, float knockbackTime, float stunTime)
    {
        // Bestimme, ob forceTransform links oder rechts von rb.position ist
        float currentKnockBackTime = 0f;
        Vector2 P0 = rb.position;
        Vector2 P2 = GetP2(P0, forceTransform, jumpWidth);
        Vector2 P1 = GetP1(P0, P2, jumpHeight);

        while (currentKnockBackTime < knockbackTime)
        {
            // Parameter t: 0 -> 1
            float t = currentKnockBackTime / knockbackTime;

            // Quadratische Bezier-Gleichung
            Vector2 pos = Mathf.Pow(1 - t, 2) * P0
                        + 2 * (1 - t) * t * P1
                        + t * t * P2;

            // Setze Position des Pfeils
            Vector2 delta = pos - rb.position;
            Vector2 neededVel = delta / Time.fixedDeltaTime;
            rb.linearVelocity = neededVel;

            currentKnockBackTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        this.rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        this.afterknockbackStateAction();

       
        //// Erschütterungs-Animation: kleiner Sprung nach oben und zurück
        //float elapsed = 0f;
        //Vector3 startPos = transform.position;
        //Vector3 peakPos = startPos + Vector3.up * jumpHeight;

        //// Nach oben bewegen
        //while (elapsed < duration / 2f)
        //{
        //    transform.position = Vector3.Lerp(startPos, peakPos, elapsed / (duration / 2f));
        //    elapsed += Time.fixedDeltaTime;
        //    yield return new WaitForFixedUpdate();
        //}
        //transform.position = peakPos;

        //// Nach unten bewegen
        //elapsed = 0f;
        //while (elapsed < duration / 2f)
        //{
        //    transform.position = Vector3.Lerp(peakPos, startPos, elapsed / (duration / 2f));
        //    elapsed += Time.fixedDeltaTime;
        //    yield return new WaitForFixedUpdate();
        //}
        ////transform.position = startPos;
        //yield return new WaitForSeconds(stunTime);
        //this.afterknockbackStateAction();
    }


}