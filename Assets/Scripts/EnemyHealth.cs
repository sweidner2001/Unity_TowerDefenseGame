using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public int currentHealth;
    public int maxHealth;
    //[SerializeField] private Slider healthBar;
    //public Gradient HealthBarGradient;
    //private Image fillArea;
    private HealthBar healthBar2;


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Awake()
    //{
    //    this.healthBar2 = GetComponentInChildren<HealthBar>();
    //    this.healthBar2.UpdateHealthBar(this.currentHealth, this.maxHealth);
    //}

    void Start()
    {
        //this.healthBar2 = GetComponentInChildren<HealthBar>();
        this.healthBar2 = transform.parent.Find("HealthBar").GetComponent<HealthBar>();

        if (healthBar2 == null)
            Debug.Log("Null: healthbar2");

        this.healthBar2.UpdateHealthBar(this.currentHealth, this.maxHealth);
        //fillArea = healthBar.fillRect.GetComponent<Image>();
        //UpdateHealthBar();
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
        this.healthBar2.UpdateHealthBar(this.currentHealth, this.maxHealth);
        //UpdateHealthBar();
    }


    //public void UpdateHealthBar()
    //{
    //    healthBar.value = (float)this.currentHealth / this.maxHealth;
    //    fillArea.color = HealthBarGradient.Evaluate(healthBar.value);
    //}
}
