using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class WaveGenerator_old : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        [Tooltip("Enemy prefab to spawn")]
        public GameObject enemyPrefab;
        [Tooltip("Number of enemies of this type to spawn")]
        public int count;
        [Tooltip("Time in seconds between each enemy spawn of this type")]
        public float spawnInterval;
    }

    [System.Serializable]
    public class SubWave
    {
        [Tooltip("Delay in seconds before this sub-wave starts (after the previous sub-wave)")]
        public float delayBeforeStart;
        [Tooltip("List of enemy-types and their spawn settings for this sub-wave")]
        public List<EnemySpawnData> spawns = new List<EnemySpawnData>();
    }

    [Header("Spawn Configuration")]
    [Tooltip("Transform where enemies will be instantiated")]
    public Transform spawnPoint;
    [Tooltip("Define your sub-waves here")]
    public List<SubWave> subWaves = new List<SubWave>();

    private void Start()
    {
        if (spawnPoint == null)
            spawnPoint = this.transform;

        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        // Loop through each sub-wave
        foreach (var subWave in subWaves)
        {
            // Wait before this sub-wave starts
            yield return new WaitForSeconds(subWave.delayBeforeStart);

            // Spawn each configured enemy-type in this sub-wave
            foreach (var spawnData in subWave.spawns)
            {
                for (int i = 0; i < spawnData.count; i++)
                {
                    Instantiate(spawnData.enemyPrefab, spawnPoint.position, Quaternion.identity);
                    yield return new WaitForSeconds(spawnData.spawnInterval);
                }
            }
        }

        // All sub-waves finished
        Debug.Log("All waves completed.");
    }
}