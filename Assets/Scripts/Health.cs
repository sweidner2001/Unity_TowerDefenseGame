using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //######################## Membervariablen ##############################
    [SerializeField]
    private int _currentHealth;

    [SerializeField]
    protected int maxHealth;
    protected HealthBar healthBar;

    protected int CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = value > this.maxHealth ? this.maxHealth : value;
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


    public virtual void ChangeHealth(int amount)
    {
        this.CurrentHealth += amount;

        if (this.CurrentHealth <= 0)
        {
            // Charakter sterben lassen:
            Destroy(this.transform.parent.gameObject);
            GetComponent<HomePoint>()?.ChangeHomePointState(false);
        }
        this.healthBar?.UpdateHealthBar(this.CurrentHealth, this.maxHealth);
    }

}
