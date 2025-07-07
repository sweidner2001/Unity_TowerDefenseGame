using System.Collections;
using UnityEngine;

public class CatBatmanLaser : MonoBehaviour
{

    public Transform laserLaunchPoint;
    public LineRenderer laserLineRenderer;
    protected float laserTime = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack(Transform enemyTransform)
    {
        //EnableLaser();
        //UpdateLaserPosition(enemyTransform);
        StartCoroutine(LaserAttack(enemyTransform));
    }

    private IEnumerator LaserAttack(Transform enemyTransform)
    {
        UpdateLaserPosition(enemyTransform);
        EnableLaser();
        float currentLaserTime = 0f;

        while (currentLaserTime < this.laserTime)
        {
            UpdateLaserPosition(enemyTransform);
            currentLaserTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        DisableLaser();
    }

    protected void UpdateLaserPosition(Transform enemyTransform)
    {
        Vector3 pos = enemyTransform.position - laserLaunchPoint.position;
        if (transform.parent.localScale.x < 0)
        {
            pos.x *= -1;
        }
        laserLineRenderer.SetPosition(1, pos);
    }


    protected void EnableLaser()
    {
        laserLineRenderer.enabled = true;
    }

    protected void DisableLaser()
    {
        laserLineRenderer.enabled = false;
    }
}
