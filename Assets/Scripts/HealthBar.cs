using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //########################### Membervariablen #############################
    protected Slider healthBarSlider;
    protected Image fillArea;
    protected ConfigHealthBar ConfigHealthBar;



    //########################### Geerbte Methoden #############################
    void Start()
    {
        //transform.parent.Find("HealthBarSlider");
        healthBarSlider = GetComponentInChildren<Slider>();
        fillArea = healthBarSlider.fillRect.GetComponentInChildren<Image>();
        this.ConfigHealthBar = Resources.Load<ConfigHealthBar>("Config/ConfigHealthBar");
    }



    //################################ Methoden ##################################
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarSlider == null)
        {
            Start();
        }
        healthBarSlider.value = (float)currentHealth / maxHealth;
        fillArea.color = ConfigHealthBar.HealthBarGradient.Evaluate(healthBarSlider.value);
    }
}
