using Assets.Scripts;
using UnityEngine;

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
        Dynamite dynamite = Instantiate(dynamitePrefab, dynamiteLaunchPoint.position, Quaternion.identity).GetComponent<Dynamite>();
        dynamite.Init(this.dynamiteConfig, this.enemyTransform, HandleDynamiteExplosion, this.ConfigTNTMan.DetectionLayer);
        return dynamite;
    }

    private void HandleDynamiteExplosion(Collider2D collision)
    {

        collision.gameObject.GetComponentInChildren<PlayerHealth>()?.ChangeHealth(-this.ConfigTNTMan.Damage);
        collision.gameObject.GetComponentInChildren<Health>()?.ChangeHealth(-this.ConfigTNTMan.Damage);

        if (this.ConfigTNTMan.KnockbackEnabled)
        {
            collision.gameObject.GetComponentInChildren<Knockback>()?.KnockbackCharacter(this.transform,
                                                                                         this.ConfigTNTMan.KnockbackForce,
                                                                                         this.ConfigTNTMan.KnockbackTime,
                                                                                         this.ConfigTNTMan.StunTime);
        }
    }
}
