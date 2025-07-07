using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBatmanLaser : MonoBehaviour
{

    public Transform laserLaunchPoint;
    public LineRenderer laserLineRenderer;
    protected float laserTime = 1;
    public GameObject startVXF;
    public GameObject endVXF;
    private List<ParticleSystem> particles = new List<ParticleSystem>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FillLists();
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack(Transform enemyTransform)
    {
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
        endVXF.transform.position = enemyTransform.position;
    }


    protected void EnableLaser()
    {
        laserLineRenderer.enabled = true;

        for(int i=0; i<particles.Count; i++)
            particles[i].Play();
    }

    protected void DisableLaser()
    {
        laserLineRenderer.enabled = false;

        for (int i = 0; i < particles.Count; i++)
            particles[i].Stop();
    }


    void FillLists()
    {
        for(int i=0; i<startVXF.transform.childCount; i++)
        {
            ParticleSystem ps = startVXF.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }
        }

        for (int i = 0; i < endVXF.transform.childCount; i++)
        {
            ParticleSystem ps = endVXF.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }
        }
    }
}
