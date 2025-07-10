using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LevelStatsManager : MonoBehaviour
{
    public static LevelStatsManager Instance { get; private set; }

    public int coinsAtStart = 200;
    public int maxEnemiesNotDestroyed = 35;
    public int coins;
    public int enemiesSurvivedCounter;
    public UIManager uiManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        ResetValues();
    }


    public void ResetValues()
    {
        coins = coinsAtStart;
        enemiesSurvivedCounter = maxEnemiesNotDestroyed;

        uiManager.UIUpdateCoins(coins);
        uiManager.UIUpdateRestEnemies(enemiesSurvivedCounter);
    }




    public void EnemyDie(int coins)
    {
        this.coins += coins;
        uiManager.UIUpdateCoins(this.coins);
    }


    public void EnemySurvived()
    {
        enemiesSurvivedCounter--;
        uiManager.UIUpdateRestEnemies(this.enemiesSurvivedCounter);

        if (enemiesSurvivedCounter <= 0)
        {
            Debug.Log("Spiel verloren!");
        }
    }
}
