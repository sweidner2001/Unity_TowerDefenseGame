using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public int currentHealth;
    public int maxHealth;
    private HealthBar healthBar;


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.healthBar = transform.parent.Find("HealthBar").GetComponent<HealthBar>();

        if (healthBar == null)
            Debug.Log("Keine HealthBar vorhanden");

        this.healthBar.UpdateHealthBar(this.currentHealth, this.maxHealth);
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
            Destroy(this.transform.parent.gameObject);
            GetComponent<Enemy_Movement2>().ChangeHomePointState(false);
        }
        this.healthBar.UpdateHealthBar(this.currentHealth, this.maxHealth);
    }

}
