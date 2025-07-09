using UnityEngine;

public class EnemySpawner2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float spawnTimer = 0f;
    private float spanwInterval = 2f;
    public GameObject Prefeb;
    public Transform SpawnTransform;

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spanwInterval;
        }
    }

    private void SpawnEnemy()
    {
        GameObject spawnedObject = Instantiate(Prefeb);
        spawnedObject.transform.position = this.transform.parent.position;
    }

}
