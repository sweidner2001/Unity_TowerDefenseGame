using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHealth : MonoBehaviour
{
    //######################## Membervariablen ##############################
    //public int currentHealth;
    //public int maxHealth;
    protected HealthBar healthBar;
    //protected UIManager uiManager;
    //[SerializeField] protected TMP_Text uiHealthText;
    //[SerializeField] protected Animator healthTextAnim;


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerStatsManager.Instance.maxHealth == 0)
            PlayerStatsManager.Instance.maxHealth = 1;


        this.healthBar = transform.parent.Find("HealthBar").GetComponent<HealthBar>();
        if (this.healthBar == null)
        {
            Debug.LogError("HealthBar component not found in parent. Please ensure it is attached to the parent GameObject.");
            return;
        }
        this.healthBar.UpdateHealthBar(PlayerStatsManager.Instance.currentHealth, PlayerStatsManager.Instance.maxHealth);

        UIUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UIUpdate()
    {
        UIManager.Instance.UIUpdateHealth(PlayerStatsManager.Instance.maxHealth, PlayerStatsManager.Instance.currentHealth);
    }


    //########################### Methoden #############################
    public void ChangeHealth(float amount)
    {
        PlayerStatsManager.Instance.currentHealth += amount;

        if (PlayerStatsManager.Instance.currentHealth < 0)
            PlayerStatsManager.Instance.currentHealth = 0;
        UIUpdate();


        // Charakter sterben lassen:
        if (PlayerStatsManager.Instance.currentHealth <= 0)
        {
            this.transform.parent.gameObject.SetActive(false);
        }
        else if (PlayerStatsManager.Instance.currentHealth > PlayerStatsManager.Instance.maxHealth)
        {
            PlayerStatsManager.Instance.currentHealth = PlayerStatsManager.Instance.maxHealth;
        }
        this.healthBar.UpdateHealthBar(PlayerStatsManager.Instance.currentHealth, PlayerStatsManager.Instance.maxHealth);

    }




}