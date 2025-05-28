using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public int currentHealth;
    public int maxHealth;
    [SerializeField] private Slider healthBar;
    public Gradient HealthBarGradient;
    private Image fillArea;


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fillArea = healthBar.fillRect.GetComponent<Image>();
        UpdateHealthBar();
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

        UpdateHealthBar();
    }


    public void UpdateHealthBar()
    {
        Debug.Log("CurrentHealth" + PlayerStatsManager.Instance.currentHealth);
        healthBar.value = (float)this.currentHealth / this.maxHealth;
        fillArea.color = HealthBarGradient.Evaluate(healthBar.value);
        Debug.Log("healthBar.value: " + (float)this.currentHealth / this.maxHealth);
    }
}
