using Assets.Scripts;
using UnityEngine;
using static UnityEngine.Rendering.STP;

public class TNTManWeapon : MonoBehaviour
{
    //######################## Membervariablen ##############################
    [SerializeField] protected GameObject dynamitePrefab;
    protected Transform dynamiteLaunchPoint;
    public ConfigTNTMan ConfigTNTMan { get; set; }
    protected ConfigDynamite dynamiteConfig;

    private Transform enemyTransform;





    //############################ Geerbte Methoden ##############################
    void Start()
    {
        this.ConfigTNTMan = transform.GetComponent<TNTMan>().GetConfig();
        this.dynamiteConfig = Resources.Load<ConfigDynamite>("Config/TNTMan/Dynamite_Std");
        this.dynamiteLaunchPoint = transform.Find("DynamiteLaunchPoint");
    }


    void Update()
    {
        
    }







    //############################ Methoden ##############################
    public void Shot()
    {
        CreateDynamite();
    }

    public void AttackEnemy(Transform enemyTransform)
    {
        this.enemyTransform = enemyTransform;
    }


    //-------------- Dynamite ------------------

    protected Dynamite CreateDynamite()
    {
        this.dynamiteConfig.DamageRadius = this.ConfigTNTMan.DamageRadius;

        Dynamite dynamite = Instantiate(dynamitePrefab, dynamiteLaunchPoint.position, Quaternion.identity).GetComponent<Dynamite>();
        dynamite.Init(this.dynamiteConfig, this.enemyTransform, HandleDynamiteExplosion, HandleDynamiteExplosionShockwave, this.ConfigTNTMan.DetectionLayer);
        return dynamite;
    }


    private void HandleDynamiteExplosion(Transform dynamiteExplosionPoint, Collider2D collisionObj)
    {
        Collider2D[] enemies = GetEnemiesInZone(dynamiteExplosionPoint, this.ConfigTNTMan.DamageRadius);
        float factor = this.ConfigTNTMan.DamageInRadiusZoneFaktor;
        float damageInZone = this.ConfigTNTMan.Damage * factor;

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.gameObject == collisionObj.gameObject)
            {
                enemy.gameObject.GetComponentInChildren<PlayerHealth>()?.ChangeHealth(-this.ConfigTNTMan.Damage);
                enemy.gameObject.GetComponentInChildren<Health>()?.ChangeHealth(-this.ConfigTNTMan.Damage);
            } 
            else
            {
                enemy.gameObject.GetComponentInChildren<PlayerHealth>()?.ChangeHealth(-damageInZone);
                enemy.gameObject.GetComponentInChildren<Health>()?.ChangeHealth(-damageInZone);
            }
        }
    }


    private void HandleDynamiteExplosionShockwave(Transform dynamiteExplosionPoint, Collider2D collisionObj)
    {
        if (!this.ConfigTNTMan.KnockbackEnabled)
        {
            return;
        }
        Collider2D[] enemies = GetEnemiesInZone(dynamiteExplosionPoint, this.ConfigTNTMan.DamageRadius);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.gameObject == collisionObj.gameObject)
            {
                    enemy.gameObject.GetComponentInChildren<Knockback>()?.KnockbackCharacter(this.transform,
                                                                                                 this.ConfigTNTMan.KnockbackForce,
                                                                                                 this.ConfigTNTMan.KnockbackTime,
                                                                                                 this.ConfigTNTMan.StunTime);
            }
            else
            {
                    enemy.gameObject.GetComponentInChildren<Knockback>()?.KnockbackCharacter(this.transform,
                                                                                             this.ConfigTNTMan.KnockbackForce * this.ConfigTNTMan.DamageInRadiusZoneFaktor,
                                                                                             this.ConfigTNTMan.KnockbackTime * this.ConfigTNTMan.DamageInRadiusZoneFaktor,
                                                                                             this.ConfigTNTMan.StunTime);
            }
        }
    }



    public Collider2D[] GetEnemiesInZone(Transform dynamiteExplosionPoint, float damageRadius)
    {
        return Physics2D.OverlapCircleAll(dynamiteExplosionPoint.position, damageRadius, this.ConfigTNTMan.DetectionLayer);
    }
}
