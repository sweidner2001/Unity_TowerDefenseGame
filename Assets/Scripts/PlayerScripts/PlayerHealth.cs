using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //######################## Membervariablen ##############################
    //public int currentHealth;
    //public int maxHealth;
    [SerializeField] private Slider healthBar;
    private Image fillArea;


    //########################### Geerbte Methoden #############################
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //this.healthBar = GetComponent<Slider>();
        fillArea = healthBar.fillRect.GetComponent<Image>();
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateHealthBar()
    {
        this.healthBar.value = (float)PlayerStatsManager.Instance.currentHealth / PlayerStatsManager.Instance.maxHealth;
        this.fillArea.color = GeneralManager.Instance.HealthBarGradient.Evaluate(healthBar.value);
    }


    //########################### Methoden #############################
    public void ChangeHealth(int amount)
    {
        PlayerStatsManager.Instance.currentHealth += amount;


        // Charakter sterben lassen:
        if (PlayerStatsManager.Instance.currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else if (PlayerStatsManager.Instance.currentHealth > PlayerStatsManager.Instance.maxHealth)
        {
            PlayerStatsManager.Instance.currentHealth = PlayerStatsManager.Instance.maxHealth;
        }
        UpdateHealthBar();
    }




}