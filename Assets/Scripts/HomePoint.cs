using Assets.Scripts;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class HomePoint : MonoBehaviour
{

    public TowerHomePoint homePoint;
    protected ISoldierBase soldierBase;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //this.enemyMovement = GetComponent<Enemy_Movement2>();
        //InitHomePoint();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        this.soldierBase = GetComponent<ISoldierBase>();
        InitHomePoint();
    }


    public void GoBackToTower()
    {
        if (this.homePoint == null)
        {
            InitHomePoint();
        }

        soldierBase.Move(this.homePoint.transform);
        if ((homePoint.transform.position - this.transform.position).magnitude <= this.homePoint.homePointRadius)
        {
            // ich befinde mich an meinen HomePoint
            soldierBase.Rb.linearVelocity = Vector2.zero;
            soldierBase.ChangeState(SoldierState.OnTower);

        }
    }



    protected void InitHomePoint()
    {
        if (this.homePoint == null)
        {
            try
            {
                this.homePoint = FindNearestAvailableHomePoint().GetComponent<TowerHomePoint>();
            }
            catch (Exception e)
            {
                // Notfalls einfach stehen bleiben, wenn kein freier HomePoint existiert
                soldierBase.ChangeState(SoldierState.Idle);
                throw e;
            }

            ChangeHomePointState(true);
        }
    }


    public void ChangeHomePointState(bool isAssigned)
    {
        if (this.homePoint != null)
        {
            this.homePoint.isAssigned = isAssigned;
        }
    }

    private Transform FindNearestAvailableHomePoint()
    {
        TowerHomePoint[] homePoints = transform.parent?.parent?.GetComponentsInChildren<TowerHomePoint>();

        if (homePoints == null || homePoints.Length == 0)
        {
            throw new Exception("Keine HomePoints gefunden, Homepoints wahrscheinlich noch nicht gerendert!");
        }

        // Den nächstgelegenen freien HomePoint finden
        var nearestPoint = homePoints
            .Where(point => point.isAssigned == false) // Nur unbesetzte HomePoints
            .OrderBy(point => Vector3.Distance(this.transform.position, point.transform.position)).FirstOrDefault(); // Kürzeste Distanz finden

        if (nearestPoint == null)
        {
            throw new Exception("Kein freier HomePoint gefunden!");
        }

        return nearestPoint.transform; // Rückgabe des Transforms oder null

    }


}