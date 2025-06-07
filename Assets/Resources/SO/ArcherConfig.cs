using UnityEngine;

[CreateAssetMenu(fileName = "ArcherConfig", menuName = "Scriptable Objects/ArcherConfig")]
public class ArcherConfig : ScriptableObject
{
    [Header("Archer Attack")]
    public float attackCooldown = 2;            // Pause zwischen 2 Attacken
    public float playerDetectionRange = 4f;
    public LayerMask detectionLayer;            // was wollen wir detektieren?

    [Header("Arrow")]
    public int damage = 2;

    [Header("Enemy Knockback after attack")]
    public float knockbackForce = 1;            // wie stark wird der Gegner zurückgeschleudert 
    public float knockbackTime = 0.15f;         // Wie lange wird der Gegner zurückgeschleudert, bis er wieder zum Stehen kommt
    public float stunTime = 0.2f;               // Wie lange bleibt der Gegner anschließend noch stehen nach dem Zurückschleudern

    [Header("Health Stats")]
    public int maxHealth = 10;


}
