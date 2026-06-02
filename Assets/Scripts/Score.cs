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
    private float startTime;
    
    public int CurrentScore => currentScore;
    
    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
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

    public void ResetScore()
    {
        coinsScore = 0;
        startTime = Time.time;
        DistanceTracker.Instance.ResetDistance();
    }
}
