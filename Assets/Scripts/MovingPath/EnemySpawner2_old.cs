using UnityEngine;

public class EnemySpawner2_old : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float spawnTimer = 0f;
    private float spanwInterval = 2f;
    public GameObject Prefeb;
    public Transform createdObjekts;

    // Update is called once per frame

    private void Start()
    {
        createdObjekts = this.transform.parent.Find("CreatedObjects");
    }
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
        GameObject spawnedObject = Instantiate(Prefeb, this.transform.parent.position, Quaternion.identity, createdObjekts);
        //GameObject spawnedObject = Instantiate(Prefeb, this.transform.parent.position, Quaternion.identity);
    }

}
