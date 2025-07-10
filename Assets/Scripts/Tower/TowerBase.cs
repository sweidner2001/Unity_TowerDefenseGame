using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public GameObject towerBuy;
    public GameObject towerTorch;
    public GameObject towerWarrior;
    public GameObject towerArcher;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);

        ReplaceTower(towerArcher);
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    public void ReplaceTower(GameObject newTowerPrefab)
    {
        GameObject newTower = Instantiate(newTowerPrefab, transform.position, transform.rotation, transform.parent);

        // Alle Childs von TowerBuy auf das neue Objekt verschieben
        List<Transform> childs = new List<Transform>();
        foreach (Transform child in transform)
            childs.Add(child);
        foreach (Transform child in childs)
            child.SetParent(newTower.transform, true);

        Destroy(gameObject);
    }
}
