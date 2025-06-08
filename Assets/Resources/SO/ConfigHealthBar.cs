using UnityEngine;

[CreateAssetMenu(fileName = "ConfigHealthBar", menuName = "Scriptable Objects/ConfigHealthBar")]
public class ConfigHealthBar : ScriptableObject
{
    [Header("HealthBar")]
    public Gradient HealthBarGradient;
}
