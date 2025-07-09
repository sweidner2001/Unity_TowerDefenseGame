using Assets.Resources.Interfaces;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigPawn", menuName = "Scriptable Objects/ConfigPawn")]
public class ConfigPawn : ConfigSoldierBase, IKnockbackShake
{

    [Header("Attack")]
    [SerializeField] protected float playerDetectionRange = 2.5f;
    [SerializeField] protected float attackCooldown = 2;            // Pause zwischen 2 Attacken
    [SerializeField] protected float maxAttackRange = 0.7f;         // Angriffsabstand zum Gegner
    [SerializeField] protected LayerMask detectionLayer;            // Wen wollen wir angreifen?


    [Header("Torch Weapon")]
    [SerializeField] protected float weaponRange = 2;               // Reichweite der Waffe
    [SerializeField] protected int damage = 1;


    [Header("Enemy Knockback after attack")]
    [SerializeField] protected bool enableKnockbackShake = true;
    [SerializeField] protected float knockbackHeight = 3;            
    [SerializeField] protected float knockbackWidth = 0.15f;         
    [SerializeField] protected float knockbackTime = 0.2f;           
    [SerializeField] protected float stunTime = 0.2f;

    [Header("Movement")]
    [SerializeField] protected float movingSpeed = 0.6f;

    [Header("Health")]
    [SerializeField] protected int maxHealth = 15;


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

    // IKnockbackShake
    public bool EnableKnockbackShake
    {
        get => enableKnockbackShake;
        set => enableKnockbackShake = value;
    }

    public float KnockbackHeight
    {
        get => knockbackHeight;
        set => knockbackHeight = value;
    }

    public float KnockbackWidth
    {
        get => knockbackWidth;
        set => knockbackWidth = value;
    }

    public float KnockbackTime
    {
        get => knockbackTime;
        set => knockbackTime = value;
    }

    public float StunTime
    {
        get => stunTime;
        set => stunTime = value;
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
