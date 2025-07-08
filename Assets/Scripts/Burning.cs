using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectState
{
    OnBurning,
    StoppEffect

}


public class Burning : MonoBehaviour
{
    //######################## Membervariablen ##############################
    protected Animator fireBackgroundAnim, fireForegroundAnim;
    protected bool burning = false;
    Transform fireBackground, fireForeground;
    protected Coroutine burningCoroutine;


    protected Dictionary<EffectState, string> stateToAnimation = new Dictionary<EffectState, string>()
    {
        { EffectState.OnBurning, "isOnBurning" },
        { EffectState.StoppEffect, "isNoAnimation" },
    };

    private EffectState _state;
    public virtual EffectState State
    {
        get => _state;
        protected set
        {
            if (fireBackgroundAnim != null && fireBackgroundAnim.runtimeAnimatorController != null &&
                fireForegroundAnim != null && fireForegroundAnim.runtimeAnimatorController != null)
            {
                if (stateToAnimation.ContainsKey(_state))
                {
                    fireBackgroundAnim.SetBool(stateToAnimation[_state], false);
                    fireForegroundAnim.SetBool(stateToAnimation[_state], false);
                }

                _state = value;

                if (stateToAnimation.ContainsKey(_state))
                {
                    fireBackgroundAnim.SetBool(stateToAnimation[_state], true);
                    fireForegroundAnim.SetBool(stateToAnimation[_state], true);
                }
            }
        }
    }



    //########################### Geerbte Methoden #############################
    void Start()
    {
        this.fireBackgroundAnim = transform.Find("EffectFireBackground").GetComponent<Animator>();
        this.fireForegroundAnim = transform.Find("EffectFireForeground").GetComponent<Animator>();

        this.fireBackground = transform.Find("EffectFireBackground");
        this.fireForeground = transform.Find("EffectFireForeground");
        fireBackground.gameObject.SetActive(false);
        fireForeground.gameObject.SetActive(false);


    }

    void Update()
    {
        
    }



    //############################ Methoden: ##########################
    public void StartBurning(float burningSeconds, float damage)
    {
        if(this.burning)
        {
            if (burningCoroutine != null)
                StopCoroutine(burningCoroutine);
        }
        StartBurningEffect();
        burningCoroutine = StartCoroutine(BurningCoroutine(burningSeconds, damage));
    }


    protected void UpdateBurningEffekt(float burningSeconds, float damage)
    {
        // Stoppe die alte Coroutine und starte eine neue mit den neuen Werten
        if (burningCoroutine != null)
            StopCoroutine(burningCoroutine);

        StartBurningEffect();
        burningCoroutine = StartCoroutine(BurningCoroutine(burningSeconds, damage));
    }

    private IEnumerator BurningCoroutine(float burningSeconds, float damage)
    {
        // Variablen:
        float currentBurningTime = 0f;
        float damagePerTick = damage / (burningSeconds / Time.fixedDeltaTime);
        float appliedDamage = 0f;

        // Laser starten:

        // Schaden abziehen und Laser bewegen:
        while (currentBurningTime < burningSeconds)
        {
            // Abbruch, wenn das Feuer gelöscht wurde:
            if (!this.burning) 
                yield break;

            // Schaden zufügen:
            GetComponent<PlayerHealth>()?.ChangeHealth(-damagePerTick);
            GetComponent<Health>()?.ChangeHealth(-damagePerTick);
            appliedDamage += damagePerTick;

            currentBurningTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Restschaden ausgleichen (wegen Rundungsfehlern)
        SetMissingDamage(damage - appliedDamage);
        StopBurning();
    }


    protected void ChangeState(EffectState state)
    {

        if (state == EffectState.StoppEffect)
        {
            fireBackground.gameObject.SetActive(false);
            fireForeground.gameObject.SetActive(false);
            this.burning = false;
        } 
        else if(!this.burning)
        {
            // Wenn schon Feuer vorhanden, dann muss ist das gameObject schon aktiv!
            this.burning = true;
            fireBackground.gameObject.SetActive(true);
            fireForeground.gameObject.SetActive(true);
        }
        this.State = state;
    }

    protected void SetMissingDamage(float missingDamage)
    {
        if (Mathf.Abs(missingDamage) > 0.1f)
        {
            GetComponent<PlayerHealth>()?.ChangeHealth(-missingDamage);
            GetComponent<Health>()?.ChangeHealth(-missingDamage);
        }
    }

    protected void StartBurningEffect()
    {
        ChangeState(EffectState.OnBurning);
    }

    public void StopBurning()
    {
        ChangeState(EffectState.StoppEffect);
    }
}
