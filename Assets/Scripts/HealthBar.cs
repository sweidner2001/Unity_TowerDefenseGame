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
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarSlider == null)
            Start();
        else if(maxHealth <= 0)
        {
            Debug.Log("Max health is zero or less, health bar will not update.");
            return;
        }

        healthBarSlider.value = (float)currentHealth / maxHealth;
        fillArea.color = configHealthBar.HealthBarGradient.Evaluate(healthBarSlider.value);
    }
}
