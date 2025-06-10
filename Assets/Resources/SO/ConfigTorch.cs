using UnityEngine;

[CreateAssetMenu(fileName = "ConfigTorch", menuName = "Scriptable Objects/ConfigTorch")]
public class ConfigTorch : ConfigSoldierBase
{
    [Header("Torch")]
    [SerializeField] protected float weaponRange = 1;               // Reichweite der Waffe


    public float WeaponRange
    {
        get => weaponRange;
        set => weaponRange = value;
    }


}