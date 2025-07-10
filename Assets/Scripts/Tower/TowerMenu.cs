using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerMenu : MonoBehaviour, IPointerExitHandler,
                                         IPointerEnterHandler
{
    [HideInInspector] public TowerBase towerBase;

    public void Init(TowerBase spot)
    {
        towerBase = spot;

        // Buttons besetzen
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            int captured = i; // lokale Kopie für die Lambda
            buttons[0].onClick.AddListener(() =>
            {
                towerBase.OnButtonClickArcher();
            });
            buttons[1].onClick.AddListener(() =>
            {
                towerBase.OnButtonClickTorch();
            });
            buttons[2].onClick.AddListener(() =>
            {
                towerBase.OnButtonClickWarrior();
            });
        }
    }




    public void OnPointerEnter(PointerEventData e)
    {
        //if (towerBase.hideMenuCoroutine != null)
        //{
        //    towerBase.StopCoroutine(towerBase.hideMenuCoroutine);
        //    towerBase.hideMenuCoroutine = null;
        //}
    }

    public void OnPointerExit(PointerEventData e)
    {
        //if (towerBase.hideMenuCoroutine == null)
        //towerBase.hideMenuCoroutine = StartCoroutine(towerBase.HideMenuWithDelay());
        //if (!data.pointerCurrentRaycast.isValid)
    //        towerBase.HideMenu();
    }
}
