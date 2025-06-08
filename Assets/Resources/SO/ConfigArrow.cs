using UnityEngine;

[CreateAssetMenu(fileName = "ConfigArrow", menuName = "Scriptable Objects/ConfigArrow")]
public class ConfigArrow : ScriptableObject
{
    [Header("Pfeil: Objekt Treffer")]
    public float lifeSpanOnHittedObject = 2;                      // Max. Lebenszeit in s des Pfeils
    public float lifeSpanOnHittedTilemap = 1;


    [Header("Pfeil: Flug-Kurve")]
    public float maxArcHeight = 2f;                            // Wie hoch der Bogen sein soll
    public float maxFlightDuration = 1.3f;                     // Zeit in Sekunden, bis der Pfeil ankommt
    public float minArcHeight = -0.5f;                         // Wie hoch der Bogen sein soll
    public float minFlightDuration = 0;                        // Zeit in Sekunden, bis der Pfeil ankommt
    public float maxDistanceFromStartPosToUpdateEnemyPos = 2;
}
