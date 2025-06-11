using UnityEngine;

[CreateAssetMenu(fileName = "ConfigWarrior", menuName = "Scriptable Objects/ConfigWarrior")]
public class ConfigWarrior : ConfigSoldierBase
{

    [Header("Attack")]
    [SerializeField] protected float playerDetectionRange = 2.5f;
    [SerializeField] protected float attackCooldown = 2;            // Pause zwischen 2 Attacken
    [SerializeField] protected float maxAttackRange = 0.7f;         // Angriffsabstand zum Gegner
    [SerializeField] protected LayerMask detectionLayer;            // Wen wollen wir angreifen?


    [Header("Torch Weapon")]
    [SerializeField] protected float weaponRange = 1;               // Reichweite der Waffe
    [SerializeField] protected int damage = 1;


    [Header("Enemy Knockback after attack")]
    [SerializeField] protected bool knockbackEnabled = true;
    [SerializeField] protected float knockbackForce = 3;            // wie stark wird der Gegner zur�ckgeschleudert 
    [SerializeField] protected float knockbackTime = 0.15f;         // Wie lange wird der Gegner zur�ckgeschleudert, bis er wieder zum Stehen kommt
    [SerializeField] protected float stunTime = 0.2f;               // Wie lange bleibt der Gegner anschlie�end noch stehen nach dem Zur�ckschleudern

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

    // Knockback
    public override bool KnockbackEnabled
    {
        get => knockbackEnabled;
        set => knockbackEnabled = value;
    }

    public override float KnockbackForce
    {
        get => knockbackForce;
        set => knockbackForce = value;
    }

    public override float KnockbackTime
    {
        get => knockbackTime;
        set => knockbackTime = value;
    }

    public override float StunTime
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
