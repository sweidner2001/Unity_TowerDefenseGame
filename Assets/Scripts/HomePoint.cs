using Assets.Scripts;
using System;
using System.Linq;
using UnityEngine;

public class HomePoint : MonoBehaviour
{

    public TowerHomePoint homePoint;
    protected Enemy_Movement2 enemyMovement;


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
        this.enemyMovement = GetComponent<Enemy_Movement2>();
        InitHomePoint();
    }


    public void GoBackToTower()
    {
        if (this.homePoint == null)
        {
            InitHomePoint();
        }

        enemyMovement.Move(this.homePoint.transform);
        if ((homePoint.transform.position - this.transform.position).magnitude <= this.homePoint.homePointRadius)
        {
            // ich befinde mich an meinen HomePoint
            enemyMovement.rb.linearVelocity = Vector2.zero;
            enemyMovement.ChangeState(SoldierState.OnTower);

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
                enemyMovement.ChangeState(SoldierState.Idle);
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
