using Assets.Resources.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class ConfigSoldierBase : ScriptableObject, IHealth, IKnockback, IAttack
{
    [Header("Attack")]
    [SerializeField] protected float playerDetectionRange = 2.5f;
    [SerializeField] protected float attackCooldown = 2;            // Pause zwischen 2 Attacken
    [SerializeField] protected float maxAttackRange = 0.7f;         // Angriffsabstand zum Gegner
    [SerializeField] protected LayerMask detectionLayer;            // Wen wollen wir angreifen?


    [Header("Torch")]
    public int damage = 1;
    //public float weaponRange = 1;               // Reichweite der Waffe


    [Header("Enemy Knockback after attack")]
    [SerializeField] protected bool knockbackEnabled = true;
    [SerializeField] protected float knockbackForce = 3;            // wie stark wird der Gegner zurückgeschleudert 
    [SerializeField] protected float knockbackTime = 0.15f;         // Wie lange wird der Gegner zurückgeschleudert, bis er wieder zum Stehen kommt
    [SerializeField] protected float stunTime = 0.2f;               // Wie lange bleibt der Gegner anschließend noch stehen nach dem Zurückschleudern

    [Header("Movement")]
    public float movingSpeed = 1;

    [Header("Health")]
    [SerializeField] protected int maxHealth = 6;




    // Knockback
    public bool KnockbackEnabled
    {
        get => knockbackEnabled;
        set => knockbackEnabled = value;
    }

    public float KnockbackForce
    {
        get => knockbackForce;
        set => knockbackForce = value;
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
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }


    // Attack (IAttack)
    public float PlayerDetectionRange
    {
        get => playerDetectionRange;
        set => playerDetectionRange = value;
    }

    public float AttackCooldown
    {
        get => attackCooldown;
        set => attackCooldown = value;
    }

    public float MaxAttackRange
    {
        get => maxAttackRange;
        set => maxAttackRange = value;
    }

    public LayerMask DetectionLayer
    {
        get => detectionLayer;
        set => detectionLayer = value;
    }
}