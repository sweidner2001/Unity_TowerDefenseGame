using Assets.Scripts;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //######################## Membervariablen ##############################
    [SerializeField]
    private float _currentHealth;

    [SerializeField]
    protected int maxHealth;
    protected HealthBar healthBar;
    protected bool isHealthBarEnabled = false;

    public float CurrentHealth
    {
        get => _currentHealth;
        protected set => _currentHealth = value > this.maxHealth ? this.maxHealth : Math.Max(value, 0);
    }


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this.maxHealth == 0)
            this.maxHealth = 1;


        this.healthBar = transform.parent.Find("HealthBar").GetComponent<HealthBar>();
        if (this.healthBar == null)
        {
            Debug.LogError("HealthBar component not found in parent. Please ensure it is attached to the parent GameObject.");
            return;
        }
        this.healthBar.UpdateHealthBar(this.CurrentHealth, this.maxHealth);
    }


    // Update is called once per frame
    void Update()
    {

    }


    //########################### Methoden #############################
    public void Init(int maxHealth)
    {
        this.maxHealth = maxHealth;
        this.CurrentHealth = maxHealth;
        this.healthBar?.UpdateHealthBar(maxHealth, maxHealth);
    }


    public virtual void ChangeHealth(float amount)
    {
        
        this.CurrentHealth += amount;

        if (this.CurrentHealth <= 0)
        {
            // Charakter sterben lassen:
            //Destroy(this.transform.parent.gameObject);
            this.GetComponent<HomePoint>()?.ChangeHomePointState(false);
            this.GetComponent<ISoldierBase>()?.Die();
        }
        //else if (this.CurrentHealth >= this.maxHealth)
        //{
        //    //EnableHealthBar(false);
        //    this.CurrentHealth = this.maxHealth;
        //} 
        //else if(isHealthBarEnabled == false)
        //{
        //    //EnableHealthBar(true);
        //}
        this.healthBar?.UpdateHealthBar(this.CurrentHealth, this.maxHealth);
    }

    public void DestroyCharakter()
    {
        Destroy(this.transform.parent.gameObject);
    }

    //protected void EnableHealthBar(bool visible)
    //{
    //    isHealthBarEnabled = visible;
    //    this.healthBar.EnableHealthBar(visible);
    //}
}
