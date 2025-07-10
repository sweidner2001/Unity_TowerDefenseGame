using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ConfigEnemySpawner", menuName = "Scriptable Objects/ConfigEnemySpawner")]
public class ConfigEnemySpawner : ScriptableObject
{
    [SerializeField] protected List<EnemyPrefabEntry> enemyPrefabs;



    //####################### Methoden ###########################
    public Dictionary<EnemyType, GameObject> GetEnemyTypePrefapMapping()
    {
        Dictionary<EnemyType, GameObject> enemyTypePrefapMapping = new Dictionary<EnemyType, GameObject>();
        foreach (var entry in enemyPrefabs)
            enemyTypePrefapMapping[entry.enemyType] = entry.prefab;

        return enemyTypePrefapMapping;
    }


    #if UNITY_EDITOR
        void OnValidate()
        {
            var types = new HashSet<EnemyType>();
            foreach (var entry in enemyPrefabs)
            {
                if (!types.Add(entry.enemyType))
                    Debug.LogWarning($"EnemyType {entry.enemyType} ist mehrfach in der Liste!");
            }
        }
    #endif
}







[System.Serializable]
public class EnemyPrefabEntry
{
    public EnemyType enemyType;
    public GameObject prefab;
}