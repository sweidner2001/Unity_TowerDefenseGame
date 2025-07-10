using UnityEngine;

public class GameStatsManager : MonoBehaviour
{


    void Awake()
    {
        StartGame();
    }

    public void StartGame()
    {
        LevelStatsManager.Instance.ResetValues();
    }

}
