using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //########################### Membervariablen #############################
    protected Slider healthBarSlider;
    protected Image fillArea;
    protected ConfigHealthBar configHealthBar;



    //########################### Geerbte Methoden #############################
    void Start()
    {
        //transform.parent.Find("HealthBarSlider");
        healthBarSlider = GetComponentInChildren<Slider>();
        fillArea = healthBarSlider.fillRect.GetComponentInChildren<Image>();
        
        this.configHealthBar = Resources.Load<ConfigHealthBar>("Config/ConfigHealthBar");

        if(configHealthBar == null)
        {
            Debug.LogError("ConfigHealthBar is not set. Please assign a ConfigHealthBar resource.");
        }
    }



    //################################ Methoden ##################################
    public void EnableHealthBar(bool enable)
    {
        // Auf Kind-Objekte zugreifen, Parent-Element funktioniert nicht!
        fillArea.enabled = enable;
        healthBarSlider.transform.Find("Background").GetComponent<Image>().enabled = enable;
    }
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarSlider == null)
            Start();
        else if (maxHealth <= 0)
        {
            Debug.Log("Max health is zero or less, health bar will not update.");
            return;
        }

        healthBarSlider.value = (float)currentHealth / maxHealth;
        fillArea.color = configHealthBar.HealthBarGradient.Evaluate(healthBarSlider.value);

        if (healthBarSlider.value == 1 || healthBarSlider.value == 0)
        {
            EnableHealthBar(false);
        }
        else if (healthBarSlider.value < 1 && fillArea.enabled == false)
        {
            EnableHealthBar(true);
        }
    }
}
