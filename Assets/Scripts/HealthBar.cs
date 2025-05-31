using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    private Image fillArea;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fillArea = healthBar.fillRect.GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 s = transform.localScale;
        //Debug.Log("##" + s);
        //s.x = 0.01f;
        //transform.localScale = s;
    }



    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.value = (float)currentHealth / maxHealth;
        fillArea.color = GeneralManager.Instance.HealthBarGradient.Evaluate(healthBar.value);
    }
}
