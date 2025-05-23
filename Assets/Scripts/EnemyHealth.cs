using UnityEngine;

public class EnemyHealth : MonoBehaviour
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
        if(this.currentHealth > this.maxHealth)
        {
            this.currentHealth = this.maxHealth;
        }
        if (this.currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
