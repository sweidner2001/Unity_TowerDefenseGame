using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    //######################## Membervariablen ##############################
    public static PlayerStatsManager Instance;

    [Header("Combat Stats")]
    public int damage = 1;
    public float attackCooldown = 1;            // Pause zwischen 2 Attacken
    public float weaponRange = 0.5f;                   // in welchen Umkreis richtet die Waffe schaden an
    public float knockbackForce = 3;            // wie stark wird der Gegner zur�ckgeschleudert 
    public float knockbackTime = 0.15f;         // Wie lange wird der Gegner zur�ckgeschleudert, bis er wieder zum Stehen kommt
    public float stunTime = 0.2f;               // Wie lange bleibt der Gegner noch stehen nach dem Zur�ckschleudern

    [Header("Movement Stats")]
    public float movingSpeed = 3;

    [Header("Health Stats")]
    public int maxHealth = 10;
    public float currentHealth = 10;


    //########################### Geerbte Methoden #############################
    private void Awake()
    {
        if(PlayerStatsManager.Instance == null)
            PlayerStatsManager.Instance = this;
        else
            // es gibt schon eine Instance von der Klasse, wir zerst�ren diese hier
            Destroy(gameObject);
    }




}
