using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //########################### Membervariablen #############################
    private Slider healthBarSlider;
    private Image fillArea;




    //########################### Geerbte Methoden #############################
    void Start()
    {
        //transform.parent.Find("HealthBarSlider");
        healthBarSlider = GetComponentInChildren<Slider>();
        fillArea = healthBarSlider.fillRect.GetComponentInChildren<Image>();

    }



    //################################ Methoden ##################################
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (healthBarSlider == null)
        {
            Start();
        }
        healthBarSlider.value = (float)currentHealth / maxHealth;
        fillArea.color = GeneralManager.Instance.HealthBarGradient.Evaluate(healthBarSlider.value);
    }
}
