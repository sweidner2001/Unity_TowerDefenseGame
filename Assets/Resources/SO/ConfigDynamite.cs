using UnityEngine;

[CreateAssetMenu(fileName = "ConfigDynamite", menuName = "Scriptable Objects/ConfigDynamite")]
public class ConfigDynamite : ScriptableObject
{
    //[Header("Pfeil: Objekt Treffer")]
    // For Visualisation only, not used in logic
    public float DamageRadius { get; set; }



    [Header("Pfeil: Flug-Kurve")]
    public float maxArcHeight = 2f;                            // Wie hoch der Bogen sein soll
    public float maxFlightDuration = 1.3f;                     // Zeit in Sekunden, bis der Projektil ankommt
    public float minArcHeight = -0.5f;                         // Wie hoch der Bogen sein soll
    public float minFlightDuration = 0;                        // Zeit in Sekunden, bis der Projektil ankommt
    public float maxDistanceFromStartPosToUpdateEnemyPos = 2;
}
