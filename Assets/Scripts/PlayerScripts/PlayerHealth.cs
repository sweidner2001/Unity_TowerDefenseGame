using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //######################## Membervariablen ##############################
    //public int currentHealth;
    //public int maxHealth;
    //[SerializeField] private Slider healthBar;


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void UpdateHealthBar()
    //{
    //    healthBar.value = PlayerStatsManager.Instance.currentHealth / PlayerStatsManager.Instance.maxHealth;
    //}


    //########################### Methoden #############################
    public void ChangeHealth(int amount)
    {
        PlayerStatsManager.Instance.currentHealth += amount;
        //UpdateHealthBar();


        // Charakter sterben lassen:
        if (PlayerStatsManager.Instance.currentHealth <= 0)
        {
            PlayerStatsManager.Instance.gameObject.SetActive(false);
        }
        else if (PlayerStatsManager.Instance.currentHealth > PlayerStatsManager.Instance.maxHealth)
        {
            PlayerStatsManager.Instance.currentHealth = PlayerStatsManager.Instance.maxHealth;
        }
    }




}