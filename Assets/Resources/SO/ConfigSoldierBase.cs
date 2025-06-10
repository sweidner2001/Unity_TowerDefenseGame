using Assets.Resources.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ConfigSoldierBase : ScriptableObject, IHealth, IKnockback, IAttack
{
    
    public abstract float MovingSpeed { get; set; }

    // IHealth
    public abstract int MaxHealth { get; set; }

    // IKnockback
    public abstract bool KnockbackEnabled { get; set; }
    public abstract float KnockbackForce { get; set; }
    public abstract float KnockbackTime { get; set; }
    public abstract float StunTime { get; set; }

    // IAttack
    public abstract float PlayerDetectionRange { get; set; }
    public abstract float AttackCooldown { get; set; }
    public abstract float MaxAttackRange { get; set; }
    public abstract LayerMask DetectionLayer { get; set; }
    public abstract int Damage { get; set; }
}