using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.STP;



public enum EnemyType
{
    BatmanCat,
    Pawn,
    TNTMan
}



public class WaveGenerator : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public ConfigEnemyWave[] waves;             // Alle Wellen im Spiel
    public Transform[] spawnPoints;             // Spawn-Punkte für Gegner

    protected int currentWaveIndex = 0;
    protected bool isSpawning = false;

    protected ConfigEnemySpawner Config;          // Konfiguration für den Gegner-Spawner
    protected Dictionary<EnemyType, GameObject> enemyTypePrefapMapping;
    protected Transform containerCreatedObjekts;          // Container für die erstellten Objekte



    //########################### Geerbte Methoden #############################
    void Start()
    {
        Config = GetConfig();
        enemyTypePrefapMapping = Config.GetEnemyTypePrefapMapping();
        containerCreatedObjekts = this.transform.parent.Find("CreatedObjects");
        StartNextWave();

    }












    //############################## Methoden: ################################
    //************************* Init: ***************************
    public ConfigEnemySpawner GetConfig()
    {
        if (Config == null)
        {
            string pfad = "Config/EnemyWave/ConfigEnemySpawner";
            Config = Resources.Load<ConfigEnemySpawner>(pfad);

            if (Config == null)
            {
                throw new Exception($"Config {pfad} konnte nicht geladen werden!");
            }
        }
        return Config;
    }



    //************************* Bedienelemente: ***************************
    public bool StartNextWave()
    {
        if (!isSpawning && currentWaveIndex < waves.Length)
        {
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            currentWaveIndex++;
            return true;
        }
        return false;
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



    //************************* Spawning: ***************************
    IEnumerator SpawnWave(ConfigEnemyWave wave)
    {
        isSpawning = true;

        foreach (SubWave subWave in wave.subWaves)
        {
            // Spawn alle Gegner in der Sub-Welle
            foreach (EnemySpawnData enemySubWave in subWave.enemyWaves)
            {
                for (int i = 0; i < enemySubWave.count; i++)
                {
                    SpawnEnemy(this.enemyTypePrefapMapping[enemySubWave.enemyType]);
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
        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, containerCreatedObjekts);
    }


}