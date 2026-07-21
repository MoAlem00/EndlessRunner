using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public enum GameState { GameOver, Paused, Playing }

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
        gameState = GameState.Playing;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void SetState(GameState newState)
    {
        gameState = newState;
        OnStateChanged?.Invoke(gameState);
    }
    public void StartGame()
    {
        SetState(GameState.Playing);
        Score.Instance.startTime = Time.time;
    }
    public IEnumerator ShowGameOverAfterDelay()
    {
        ProfileManager.Instance.SaveProfile(ProfileManager.Instance.ActiveSlotIndex);
        yield return new WaitForSeconds(2f);
        SetState(GameState.GameOver);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        ProfileManager.Instance.OnApplicationQuit();
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        ProfileManager.Instance.OnApplicationQuit();
        Application.Quit();
        #endif
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MoAlemScene");
    }
    
    public void PauseGame()
    {
        SetState(GameState.Paused);
        Time.timeScale = 0f;
    }
    
    public void ResumeGame()
    {
        SetState(GameState.Playing);
        Time.timeScale = 1f;
    }

    public bool IsPlaying()
    {
        return gameState == GameState.Playing;
    }
}
