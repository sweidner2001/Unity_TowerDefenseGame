using Assets.Resources.Interfaces;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigArcher", menuName = "Scriptable Objects/ConfigArcher")]
public class ConfigArcher : ConfigSoldierBase, IKnockbackClassic
{
    [Header("Archer Attack")]
    [SerializeField] protected float playerDetectionRange = 4f;
    [SerializeField] protected float attackCooldown = 2;            // Pause zwischen 2 Attacken
    [SerializeField] protected LayerMask detectionLayer;            // Wen wollen wir angreifen?


    [Header("Arrow")]
    [SerializeField] protected int damage = 2;


    [Header("Enemy Knockback after attack")]
    [SerializeField] protected bool enableKnockbackClassic = true;
    [SerializeField] protected float knockbackForce = 1;            // wie stark wird der Gegner zurückgeschleudert 
    [SerializeField] protected float knockbackTime = 0.15f;         // Wie lange wird der Gegner zurückgeschleudert, bis er wieder zum Stehen kommt
    [SerializeField] protected float stunTime = 0.2f;               // Wie lange bleibt der Gegner anschließend noch stehen nach dem Zurückschleudern


    [Header("Health")]
    [SerializeField] protected int maxHealth = 8;



    //##################### Properties ######################
    //********* Abweichende Logik: ***********
    public override float MaxAttackRange
    {
        get => playerDetectionRange;
        set => playerDetectionRange = value;
    }


    //********* Normale Logik: ***********
    // Weapon
    public override int Damage
    {
        get => damage;
        set => damage = value;

    }

    
  

    // Knockback
    public bool EnableKnockbackClassic
    {
        get => enableKnockbackClassic;
        set => enableKnockbackClassic = value;
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

    public override LayerMask DetectionLayer
    {
        get => detectionLayer;
        set => detectionLayer = value;
    }

    // Movement
    public override float MovingSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }



    //[Header("Archer Attack")]
    //public float attackCooldown = 2;            // Pause zwischen 2 Attacken
    //public float playerDetectionRange = 4f;
    //public LayerMask detectionLayer;            // Wen wollen wir angreifen?

    //[Header("Arrow")]
    //public int damage = 2;

    //[Header("Enemy Knockback after attack")]
    //public bool knockbackEnabled = true;
    //public float knockbackForce = 1;            // wie stark wird der Gegner zurückgeschleudert 
    //public float knockbackTime = 0.15f;         // Wie lange wird der Gegner zurückgeschleudert, bis er wieder zum Stehen kommt
    //public float stunTime = 0.2f;               // Wie lange bleibt der Gegner anschließend noch stehen nach dem Zurückschleudern

    //[Header("Health")]
    //public int maxHealth = 10;


}
