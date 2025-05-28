using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //######################## Membervariablen ##############################
    //public int currentHealth;
    //public int maxHealth;



    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //########################### Methoden #############################
    public void ChangeHealth(int amount)
    {
        PlayerStatsManager.Instance.currentHealth += amount;


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