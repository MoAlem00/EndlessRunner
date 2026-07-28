using UnityEngine;

public class GoalManager : MonoBehaviour
{
    [SerializeField] private Goal[] allGoals; //array for all the goals
    public GoalProgress GetProgress(string goalId) => GetOrCreateProgress(goalId);
    public Goal[] AllGoals => allGoals;
    public static GoalManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    public void CheckGoals()
    {
        if (ProfileManager.Instance.ActiveProfile == null) return;
        foreach (Goal goal in allGoals)
        {
            int runValue = GetRunValue(goal.type);
            GoalProgress progress = GetOrCreateProgress(goal.goalId);

            if (runValue > progress.bestValue)
                progress.bestValue = runValue;
            //Debug.Log($"{goal.goalId}: {progress.bestValue}/{goal.target}");
        }
        
    }
    
    private int GetRunValue(GoalType type)
    {
        switch (type)
        {
            case GoalType.CoinsInRun:     return Score.Instance.CoinsCollected;
            case GoalType.DistanceInRun:  return Mathf.FloorToInt(DistanceTracker.Instance.GetDistance());
            case GoalType.PowerUpsInRun:  return Score.Instance.PowerUpsCollected;
            default: return 0;
        }
    }
    
    private GoalProgress GetOrCreateProgress(string goalId)
    {
        var list = ProfileManager.Instance.ActiveProfile.goalProgress;
        foreach (var goalProgress in list)
            if (goalProgress.goalId == goalId) return goalProgress;

        var newProgress = new GoalProgress { goalId = goalId, bestValue = 0, claimed = false };
        list.Add(newProgress);
        return newProgress;
    }
    
    public void ClaimAllRewards()
    {
        int totalReward = 0;
        foreach (Goal goal in allGoals)
        {
            GoalProgress p = GetOrCreateProgress(goal.goalId);
            if (p.bestValue >= goal.target && !p.claimed)
            {
                totalReward += goal.rewardCoins;
                p.claimed = true;
            }
        }
        if (totalReward > 0) ProfileManager.Instance.AddCoins(totalReward);
    }
}
