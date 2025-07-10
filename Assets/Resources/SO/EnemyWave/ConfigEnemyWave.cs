using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ConfigEnemyWave", menuName = "Scriptable Objects/ConfigEnemyWave")]
public class ConfigEnemyWave : ScriptableObject
{
    [Header("Hauptwelle Konfigurieren")]
    public SubWave[] subWaves;
}

[System.Serializable]
public class SubWave
{
    [Header("Subwelle und ihre gespawnten Objekte")]
    public EnemySpawnData[] enemyWaves; // Alle Gegner-Typen in dieser Sub-Welle
    [Header("Wartezeit bis zur nächsten Sub-Welle")]
    public float subWaveDelay;           // Wartezeit bis zur nächsten Sub-Welle
}

[System.Serializable]
public class EnemySpawnData
{
    [Tooltip("Gespawntes Objekt")]
    public EnemyType enemyType;

    [Tooltip("Anzahl Objekte die gespawned werden sollen")]
    public int count;             // Wie viele von diesem Gegner
    [Tooltip("Zeitabstand zwischen den Spawns")]
    public float delay;           // Zeitabstand zwischen den Spawns
}




