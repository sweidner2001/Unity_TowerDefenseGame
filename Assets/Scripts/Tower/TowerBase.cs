using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TowerType
{
    Torch,
    Warrior,
    Archer
}


public class TowerBase : MonoBehaviour,
                         IPointerEnterHandler,
                         IPointerExitHandler,
                         IPointerClickHandler
{
    public GameObject towerToBuy_WithoutHomePoint;
    public GameObject towerTorch;
    public GameObject towerWarrior;
    public GameObject towerArcher;

    [Header("UI‑Prefab mit 3 Buttons")]
    public GameObject radialMenuPrefab;

    [Header("Turm‑Prefabs in Reihenfolge der Buttons")]
    public GameObject[] towerPrefabs;   // Länge = 3

    private GameObject currentMenuInstance;

    public Coroutine hideMenuCoroutine;
    private float hoverStayTime = 2f;


    void Start()
    {
        //ReplaceTower(towerTorch);
    }

    void Update()
    {
        
    }

    public void OnButtonClickArcher()
    {
        ReplaceTower(towerArcher);
    }

    public void OnButtonClickTorch()
    {
        ReplaceTower(towerTorch);
    }

    public void OnButtonClickWarrior()
    {
        ReplaceTower(towerWarrior);
    }

    public void OnButtonClickDestroyTower()
    {
        ReplaceTower(towerToBuy_WithoutHomePoint);
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


    


    /* ---------- Hover ---------- */
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Menü anzeigen, wenn es noch nicht da ist
        if (currentMenuInstance == null)
            ShowMenu();

        // Wenn eine Ausblendung geplant war, abbrechen
        if (hideMenuCoroutine != null)
        {
            StopCoroutine(hideMenuCoroutine);
            hideMenuCoroutine = null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Menü nicht sofort schließen, sondern mit Verzögerung
        if (currentMenuInstance != null && hideMenuCoroutine == null)
            hideMenuCoroutine = StartCoroutine(HideMenuWithDelay());
    }

    /* ---------- Klick (Backup‑Lösung) ---------- */
    public void OnPointerClick(PointerEventData eventData)
    {
        // Für Touchgeräte: Einfaches Tap öffnet Menü
        if (currentMenuInstance == null)
            ShowMenu();
    }

    /* ---------- Menü‑Steuerung ---------- */
    private void ShowMenu()
    {
        currentMenuInstance = Instantiate(radialMenuPrefab, transform.position, Quaternion.identity, transform);
        currentMenuInstance.GetComponent<TowerMenu>().Init(this);
    }

    public void HideMenu()
    {
        if (currentMenuInstance != null)
            Destroy(currentMenuInstance);
    }

    /* ---------- Wird von TowerMenu aufgerufen ---------- */
    public void BuildTower(int index)
    {
        if (index < 0 || index >= towerPrefabs.Length) return;

        // Geldabfrage etc. könntest du hier einbauen
        Instantiate(towerPrefabs[index], transform.position, Quaternion.identity, transform.parent);
        Destroy(gameObject);               // Platzhalter verschwindet
    }




    public IEnumerator HideMenuWithDelay()
    {
        yield return new WaitForSeconds(hoverStayTime);

        HideMenu();
        hideMenuCoroutine = null;
    }

}
