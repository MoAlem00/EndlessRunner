using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using Event = Unity.Services.Analytics.Event;

// How to use it:
// Simply call AnalyticsManager.Instance.* where "*" is the functions below:
//   - LogRunStarted(difficulty, seed)       -> call when a run starts.
//   - LogRunEnded(score, coins, distance)   -> call when the run ends.
//   - LogRewardClaimed(source, amount)      -> call when a daily or goal reward is claimed.
//   - LogGoalCompleted(goalId, rewardType)  -> call when a goal is claimed.
//   - Session start and 100m milestones are tracked on their own, no need to call those.
public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    private const int MilestoneStepMeters = 100;
    private bool isReady = false;
    private int lastMilestoneBucket = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            // Obsolete in favor of EndUserConsent.SetConsentState, which needs a framework we don't have.
            // This is still the documented way to start collection for this package version without one.
            // For now, suppressed.
#pragma warning disable CS0618
            AnalyticsService.Instance.StartDataCollection();
#pragma warning restore CS0618
            isReady = true;
            LogSessionStart();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[AnalyticsManager] Unity Services failed to initialize: {e.Message}");
        }
    }

    private void Update()
    {
        // Track distance milestones while a run is playing.
        if (!isReady) return;
        if (GameManager.Instance == null || !GameManager.Instance.IsPlaying()) return;
        if (DistanceTracker.Instance == null) return;

        int currentBucket = Mathf.FloorToInt(DistanceTracker.Instance.GetDistance() / MilestoneStepMeters);
        if (currentBucket > lastMilestoneBucket)
        {
            lastMilestoneBucket = currentBucket;
            LogMilestoneReached(currentBucket * MilestoneStepMeters);
        }
    }

    // Standard metric: fired once per app session, right after the SDK is ready.
    private void LogSessionStart()
    {
        Record(new SessionStartEvent
        {
            AppVersion = Application.version,
            Platform = Application.platform.ToString()
        });
    }

    public void LogRunStarted(string difficulty, int seed)
    {
        lastMilestoneBucket = 0;
        Record(new RunStartedEvent { Difficulty = difficulty, RunSeed = seed });
    }

    public void LogRunEnded(int score, int coins, float distance)
    {
        Record(new RunEndedEvent { Score = score, Coins = coins, Distance = Mathf.FloorToInt(distance) });
    }

    private void LogMilestoneReached(int distanceMeters)
    {
        Record(new MilestoneReachedEvent { Milestone = distanceMeters });
    }

    public void LogRewardClaimed(string source, int amount)
    {
        Record(new RewardClaimedEvent { Source = source, Amount = amount });
    }

    public void LogGoalCompleted(string goalId, string rewardType)
    {
        Record(new GoalCompletedEvent { GoalId = goalId, RewardType = rewardType });
    }

    private void Record(Event analyticsEvent)
    {
        if (!isReady)
        {
            Debug.LogWarning("[AnalyticsManager] Skipped event, Unity Services not ready yet.");
            return;
        }
        AnalyticsService.Instance.RecordEvent(analyticsEvent);
    }
}
