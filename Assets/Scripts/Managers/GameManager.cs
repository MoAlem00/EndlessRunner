using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { MainMenu, GameOver, Paused, Playing }
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState gameState { get; private set; }
    public event Action<GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartCoroutine(AudioManager.Instance.PlayShuffleMusic());
        gameState = GameState.MainMenu;
        Time.timeScale = 0f;
    }

    private void SetState(GameState newState)
    {
        gameState = newState;
        OnStateChanged?.Invoke(gameState);
    }
    public void StartGame()
    {
        SetState(GameState.Playing);
        Time.timeScale = 1f;
    }
    public IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        SetState(GameState.GameOver);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ResumeGame()
    {
        SetState(GameState.Playing);
        Time.timeScale = 1f;
    }
    
    public void PauseGame()
    {
        SetState(GameState.Paused);
        Time.timeScale = 0f;
    }
}
