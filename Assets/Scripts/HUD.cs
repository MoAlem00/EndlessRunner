using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text coinsCounterText;
    [SerializeField] private Image[] hearts;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text totalTimeText;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TMP_Text distanceText;

    void Start()
    {
        Score.Instance.OnCoinsChanged += UpdateCoins;
        Score.Instance.OnScoreChanged += UpdateScore;
        Health.Instance.OnLivesChanged += UpdateHearts;
        GameManager.Instance.OnStateChanged += HandleStateChanged;
        HandleStateChanged(GameManager.Instance.gameState);
    }

    private void Update()
    {
        UpdateDistance();
    }

    void OnDestroy()
    {
        Score.Instance.OnCoinsChanged -= UpdateCoins; 
        Score.Instance.OnScoreChanged -= UpdateScore;
        Health.Instance.OnLivesChanged -= UpdateHearts;
        GameManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    void UpdateScore(int score)
    {
        scoreText.text = "" + score;
    }

    void UpdateHearts(int lives)
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < lives;
    }

    private void UpdateCoins(int coins)
    {
        coinsCounterText.text = "" + coins;
    }

    private void UpdateDistance()
    {
        distanceText.text = "" + Mathf.FloorToInt(DistanceTracker.Instance.GetDistance()) + " m";
    }

    void HandleStateChanged(GameState state)
    {
        gameOverMenu.SetActive(state == GameState.GameOver);
        if (state == GameState.GameOver)
        {
            finalScoreText.text = "SCORE: " + Score.Instance.CurrentScore;
            totalTimeText.text  = "TIME: " + Mathf.FloorToInt(Score.Instance.TimeSinceStart) + " s";
        }
        mainMenu.SetActive(state == GameState.MainMenu);
        pauseMenu.SetActive(state == GameState.Paused);
        pauseButton.interactable = state == GameState.Playing;
    }
    
}
