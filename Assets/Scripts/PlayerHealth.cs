using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public int currentHealth;
    public int maxHealth;



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
        this.currentHealth += amount;


        // Charakter sterben lassen:
        if(currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }




}