using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image[] hearts;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text totalTimeText;

    void Start()
    {
        Score.Instance.OnScoreChanged += UpdateScore;
        Health.Instance.OnLivesChanged += UpdateHearts;
        GameManager.Instance.OnStateChanged += HandleStateChanged;
        HandleStateChanged(GameManager.Instance.gameState);
    }

    void OnDestroy()
    {
        Score.Instance.OnScoreChanged -= UpdateScore;
        Health.Instance.OnLivesChanged -= UpdateHearts;
        GameManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateHearts(int lives)
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < lives;
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
    }
}
