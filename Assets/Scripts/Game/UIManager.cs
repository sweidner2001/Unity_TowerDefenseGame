using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] protected Animator uiTextHealthAnim;
    [SerializeField] protected Animator uiTextRestEnemiesAnim;
    [SerializeField] protected TMP_Text uiTextHealth;
    [SerializeField] protected TMP_Text uiTextCoins;
    [SerializeField] protected TMP_Text uiTextRestEnemies;
    [SerializeField] protected TMP_Text uiTextRound;
    [SerializeField] protected Button nextWaveButton;

    void Awake()
    {
        GetInstance();
    }


    public UIManager GetInstance()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        return Instance;
    }


    public void UIUpdateHealth(int maxHelath, float currentHealth)
    {
        uiTextHealthAnim?.Play("TextUpdate");
        uiTextHealth.text = $"HP: {maxHelath} / {Math.Round(currentHealth, 1)}";
    }


    public void UIUpdateCoins(int currentCoins)
    {
        uiTextCoins.text = $"{currentCoins}";
    }

    public void UIUpdateRestEnemies(int restEnemies)
    {
        uiTextRestEnemiesAnim?.Play("TextUpdate");
        uiTextRestEnemies.text = $"{restEnemies}";
    }


    public void UIUpdateCurrentRound(int maxRound, int currentRound)
    {
        // Alle Runden gespielt, Button deaktivieren:
        if (currentRound > maxRound)
        {
            UIEnablePlayButton(false);
            return;
        }
        uiTextRound.text = $"Runde: {currentRound}/{maxRound}";
    }


    public void UIEnablePlayButton(bool activ)
    {
        // Button aktiveren/deaktiveren:
        if (nextWaveButton != null)
            nextWaveButton.interactable = activ;
    }
}
