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
    protected List<Animator> effectAnimators = new List<Animator>();
    protected List<Transform> effectObjects = new List<Transform>();


    protected Dictionary<EffectState, string> stateToAnimation = new Dictionary<EffectState, string>()
    {
        { EffectState.OnBurning, "isOnBurning" },
        { EffectState.StoppEffect, "isNoAnimation" },
    };

    private EffectState _state;
    public virtual EffectState StateEffect
    {
        get => _state;
        protected set
        {
            if (fireBackgroundAnim != null && fireBackgroundAnim.runtimeAnimatorController != null &&
                fireForegroundAnim != null && fireForegroundAnim.runtimeAnimatorController != null)
            {
                if (stateToAnimation.ContainsKey(_state))
                {
                    foreach (Animator anim in effectAnimators)
                        anim.SetBool(stateToAnimation[_state], false);
                }

                _state = value;

                if (stateToAnimation.ContainsKey(_state))
                {
                    foreach (Animator anim in effectAnimators)
                        anim.SetBool(stateToAnimation[_state], true);
                }
            }
        }
    }




    //########################### Geerbte Methoden #############################
    void Start()
    {
        // Namen der gewünschten Effekte
        string[] effectNames = { "EffectFireBackground", "EffectFireForeground" };

        // Effekte suchen und zur Liste hinzufügen
        foreach (var name in effectNames)
        {
            Transform obj = transform.Find(name);
            if (obj != null)
            {
                effectObjects.Add(obj);
                obj.gameObject.SetActive(false);

                // Animator hinzufügen, falls vorhanden
                Animator anim = obj.GetComponent<Animator>();
                if (anim != null)
                    effectAnimators.Add(anim);
            }
        }
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


    protected void ChangeEffectState(EffectState state)
    {

        if (state == EffectState.StoppEffect)
        {
            foreach (Transform obj in effectObjects)
                obj.gameObject.SetActive(false);

            this.burning = false;
        } 
        else if(!this.burning)
        {
            // Wenn schon Feuer vorhanden, dann muss ist das gameObject schon aktiv!
            this.burning = true;
            foreach (Transform obj in effectObjects)
                obj.gameObject.SetActive(true);
        }
        this.StateEffect = state;
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
        this.ChangeEffectState(EffectState.OnBurning);
    }

    public void StopBurning()
    {
        this.ChangeEffectState(EffectState.StoppEffect);
    }
}
