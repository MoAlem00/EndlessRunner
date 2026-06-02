using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image[] hearts;

    void Start()
    {
        Score.Instance.OnScoreChanged += UpdateScore;
        Health.Instance.OnLivesChanged += UpdateHearts;
    }

    void OnDestroy()
    {
        Score.Instance.OnScoreChanged -= UpdateScore;
        Health.Instance.OnLivesChanged -= UpdateHearts;
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
}
