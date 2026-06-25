using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance;
    
    private int currentScore = 0;
    private float distanceFactor = 1.2f;
    private float timeFactor = 1.1f;
    private float timeSinceStart = 0;
    private int coinsScore = 0;
    public int coinsCollected = 0;
    public float startTime;
    private bool stopTimeScore = false;

    public int CoinsCollected => coinsCollected;
    public int CurrentScore => currentScore;
    public float TimeSinceStart => timeSinceStart;
    public event Action<int> OnScoreChanged;
    public event Action<int> OnCoinsChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // startTime = Time.time;
        Health.Instance.OnDeath += () => stopTimeScore = true;
    }

    private void Update()
    {
        if(!GameManager.Instance.IsPlaying()) return;
        if (stopTimeScore) return;
        timeSinceStart =  Time.time - startTime;
        int total = coinsScore + Mathf.FloorToInt(DistanceTracker.Instance.GetDistance() * distanceFactor) + Mathf.FloorToInt(timeSinceStart * timeFactor);

        if (total != currentScore)
        {
            currentScore = total;
            OnScoreChanged?.Invoke(currentScore);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        coinsScore += scoreToAdd;
    }

    public void CollectCoin()
    {
        coinsCollected++;
        OnCoinsChanged?.Invoke(coinsCollected);
    }

    public void ResetScore()
    {
        coinsScore = 0;
        coinsCollected = 0;
        startTime = Time.time;
        DistanceTracker.Instance.ResetDistance();
    }
}
