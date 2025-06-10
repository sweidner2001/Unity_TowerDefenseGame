using Assets.Resources.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

public class ConfigSoldierBase : ScriptableObject, IHealth
{
    [Header("Torch Attack")]
    public float playerDetectionRange = 2.5f;
    public float attackCooldown = 2;            // Pause zwischen 2 Attacken
    public float maxAttackRange = 0.7f;         // Angriffsabstand zum Gegner
    public LayerMask detectionLayer;            // Wen wollen wir angreifen?


    [Header("Torch")]
    public int damage = 1;
    //public float weaponRange = 1;               // Reichweite der Waffe


    [Header("Enemy Knockback after attack")]
    public bool knockbackEnabled = true;
    public float knockbackForce = 3;            // wie stark wird der Gegner zur�ckgeschleudert 
    public float knockbackTime = 0.15f;         // Wie lange wird der Gegner zur�ckgeschleudert, bis er wieder zum Stehen kommt
    public float stunTime = 0.2f;               // Wie lange bleibt der Gegner anschlie�end noch stehen nach dem Zur�ckschleudern

    [Header("Movement")]
    public float movingSpeed = 1;

    [Header("Health")]
    [SerializeField] protected int maxHealth = 6;
    public int MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }
}