using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBatmanLaser : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public Transform laserLaunchPoint;
    public LineRenderer laserLineRenderer;
    public GameObject startVXF;
    public GameObject endVXF;
    private List<ParticleSystem> particles = new List<ParticleSystem>();
    protected ConfigCat config;


    //########################### Geerbte Methoden #############################
    void Start()
    {
        InitParticleSystem();
        DisableLaser();


        this.config = this.GetComponentInParent<CatBatman>().GetConfig();
    }

    void Update()
    {

    }


    //######################## Methoden: ##########################
    protected void InitParticleSystem()
    {
        for (int i = 0; i < startVXF.transform.childCount; i++)
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


    //********************* Attack ***********************
    public void Attack(Transform enemyTransform)
    {
        StartCoroutine(LaserAttack(enemyTransform));
    }

    private IEnumerator LaserAttack(Transform enemyTransform)
    {
        // Variablen:
        float currentLaserTime = 0f;
        float damagePerTick = this.config.Damage / (this.config.LaserTime / Time.fixedDeltaTime);
        float appliedDamage = 0f;

        // Laser starten:
        UpdateLaserPosition(enemyTransform);
        EnableLaser();

        // Schaden abziehen und Laser bewegen:
        while (currentLaserTime < this.config.LaserTime)
        {
            UpdateLaserPosition(enemyTransform);

            // Schaden zufügen:
            enemyTransform.GetComponent<PlayerHealth>()?.ChangeHealth(-damagePerTick);
            enemyTransform.GetComponent<Health>()?.ChangeHealth(-damagePerTick);
            appliedDamage += damagePerTick;

            currentLaserTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Restschaden ausgleichen (wegen Rundungsfehlern)
        float missingDamage = this.config.Damage - appliedDamage;
        if (Mathf.Abs(missingDamage) > 0.1f)
        {
            enemyTransform.GetComponent<PlayerHealth>()?.ChangeHealth(-missingDamage);
            enemyTransform.GetComponent<Health>()?.ChangeHealth(-missingDamage);
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




    //************************ Laser + Particle System ***********************
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



}
