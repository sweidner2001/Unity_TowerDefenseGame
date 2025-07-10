using Assets.Resources.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ConfigSoldierBase : ScriptableObject, IHealth, IMove, IAttack
{
    // IMove
    public abstract float MovingSpeed { get; set; }

    // IHealth
    public abstract int MaxHealth { get; set; }
    [field: SerializeField] public int CoinsOnDeath { get; set; } = 0;

    // IAttack
    public abstract float PlayerDetectionRange { get; set; }
    public abstract float AttackCooldown { get; set; }
    public abstract float MaxAttackRange { get; set; }
    public abstract LayerMask DetectionLayer { get; set; }
    public abstract int Damage { get; set; }
}