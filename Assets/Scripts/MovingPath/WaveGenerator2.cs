using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySubWave
{
    public GameObject enemyPrefab; // Der Gegner der gespawnt werden soll
    public int count;             // Wie viele von diesem Gegner
    public float delay;           // Zeitabstand zwischen den Spawns
}

[System.Serializable]
public class SubWave
{
    public EnemySubWave[] enemySubWaves; // Alle Gegner-Typen in dieser Sub-Welle
    public float subWaveDelay;           // Wartezeit bis zur nächsten Sub-Welle
}

[System.Serializable]
public class Wave
{
    public SubWave[] subWaves; // Alle Sub-Wellen in dieser Welle
}

public class WaveGenerator2 : MonoBehaviour
{
    public Wave[] waves; // Alle Wellen im Spiel
    public Transform[] spawnPoints; // Spawn-Punkte für Gegner

    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    void Start()
    {
        StartNextWave();
    }

    public void StartNextWave()
    {
        if (!isSpawning && currentWaveIndex < waves.Length)
        {
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            currentWaveIndex++;
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        isSpawning = true;

        foreach (SubWave subWave in wave.subWaves)
        {
            // Spawn alle Gegner in der Sub-Welle
            foreach (EnemySubWave enemySubWave in subWave.enemySubWaves)
            {
                for (int i = 0; i < enemySubWave.count; i++)
                {
                    SpawnEnemy(enemySubWave.enemyPrefab);
                    yield return new WaitForSeconds(enemySubWave.delay);
                }
            }

            // Warte bis zur nächsten Sub-Welle
            yield return new WaitForSeconds(subWave.subWaveDelay);
        }

        isSpawning = false;
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    // Optional: Für Debugging oder UI-Anzeige
    public int GetCurrentWaveNumber()
    {
        return currentWaveIndex + 1;
    }

    public int GetTotalWaves()
    {
        return waves.Length;
    }
}