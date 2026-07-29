using System;
using UnityEngine;

public class DailyRewardManager : MonoBehaviour
{
    public static DailyRewardManager Instance;
    [SerializeField] private int rewardAmount = 100;
    [SerializeField] private int rewardHours = 1; // must be 24 for testing!

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public bool IsRewardAvailable()
    {
        string lastClaim = PlayerPrefs.GetString("LastRewardClaim", "");
        if (string.IsNullOrEmpty(lastClaim)) return true;

        DateTime last = DateTime.Parse(lastClaim);
        return (DateTime.Now - last).TotalMinutes >= rewardHours;
    }

    public void ClaimReward()
    {
        ProfileManager.Instance.AddCoins(rewardAmount);
        PlayerPrefs.SetString("LastRewardClaim", DateTime.Now.ToString("o"));
        AnalyticsManager.Instance?.LogRewardClaimed("daily", rewardAmount);
    }
}
