using UnityEngine;

[CreateAssetMenu(fileName = "ConfigCat", menuName = "Scriptable Objects/ConfigCat")]
public class ConfigCat : ConfigSoldierBase
{

    [Header("Attack")]
    [SerializeField] protected float playerDetectionRange = 2.5f;
    [SerializeField] protected float attackCooldown = 2;            // Pause zwischen 2 Attacken
    [SerializeField] protected float maxAttackRange = 0.7f;         // Angriffsabstand zum Gegner
    [SerializeField] protected LayerMask detectionLayer;            // Wen wollen wir angreifen?


    [Header("Torch Weapon")]
    [SerializeField] protected float weaponRange = 1;               // Reichweite der Waffe
    [SerializeField] protected int damage = 1;


    [Header("Movement")]
    [SerializeField] protected float movingSpeed = 1;

    [Header("Health")]
    [SerializeField] protected int maxHealth = 6;


    // Weapon
    public override int Damage
    {
        get => damage;
        set => damage = value;

    }
    public float WeaponRange
    {
        get => weaponRange;
        set => weaponRange = value;
    }

    // Movement
    public override float MovingSpeed
    {
        get => movingSpeed;
        set => movingSpeed = value;

    }




    // Health
    public override int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }


    // Attack (IAttack)
    public override float PlayerDetectionRange
    {
        get => playerDetectionRange;
        set => playerDetectionRange = value;
    }

    public override float AttackCooldown
    {
        get => attackCooldown;
        set => attackCooldown = value;
    }

    public override float MaxAttackRange
    {
        get => maxAttackRange;
        set => maxAttackRange = value;
    }

    public override LayerMask DetectionLayer
    {
        get => detectionLayer;
        set => detectionLayer = value;
    }
}
