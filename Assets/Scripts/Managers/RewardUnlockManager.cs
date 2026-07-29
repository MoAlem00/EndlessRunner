using UnityEngine;

public class RewardUnlockManager : MonoBehaviour
{
    public static RewardUnlockManager Instance;

    [SerializeField] private Goal[] allGoals;

    public Goal[] AllGoals => allGoals;
    public float CoinMultiplierBonus { get; private set; }
    public bool HasStartingShield { get; private set; }

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

    private void Start()
    {
        if (ProfileManager.Instance != null)
            ProfileManager.Instance.OnProfileLoaded += _ => RecalculateUpgrades();
        RecalculateUpgrades();
    }

    public GoalProgress GetProgress(string goalId)
    {
        ProfileData profile = ProfileManager.Instance != null ? ProfileManager.Instance.ActiveProfile : null;
        if (profile == null) return new GoalProgress { goalId = goalId, bestValue = 0, claimed = false };

        foreach (GoalProgress p in profile.goalProgress)
            if (p.goalId == goalId) return p;

        return new GoalProgress { goalId = goalId, bestValue = 0, claimed = false };
    }

    public bool TryClaimGoal(string goalId)
    {
        ProfileData profile = ProfileManager.Instance != null ? ProfileManager.Instance.ActiveProfile : null;
        if (profile == null) return false;

        Goal goal = FindGoal(goalId);
        if (goal == null) return false;

        GoalProgress progress = FindOrCreateProgress(profile, goalId);
        if (progress.claimed || progress.bestValue < goal.target) return false;

        progress.claimed = true;
        ProfileManager.Instance.AddCoins(goal.rewardCoins);

        RecalculateUpgrades();

        AnalyticsManager.Instance?.LogGoalCompleted(goal.goalId, goal.rewardType.ToString());
        AnalyticsManager.Instance?.LogRewardClaimed("goal", goal.rewardCoins);
        return true;
    }

    /// Check if unlocked, return true if it lacks a theme to the item, therefore unlocked.
    public bool IsThemeUnlocked(string themeName)
    {
        if (string.IsNullOrEmpty(themeName) || allGoals == null) return true;

        foreach (Goal goal in allGoals)
        {
            if (goal.rewardType == RewardType.ThemeUnlock && goal.unlockThemeId == themeName)
                return GetProgress(goal.goalId).claimed;
        }
        return true;
    }

    private void RecalculateUpgrades()
    {
        CoinMultiplierBonus = 0f;
        HasStartingShield = false;
        if (allGoals == null) return;

        foreach (Goal goal in allGoals)
        {
            if (!GetProgress(goal.goalId).claimed) continue;

            switch (goal.rewardType)
            {
                case RewardType.CoinMultiplierUpgrade:
                    CoinMultiplierBonus += goal.upgradeAmount;
                    break;
                case RewardType.StartingShieldUpgrade:
                    HasStartingShield = true;
                    break;
            }
        }
    }

    private Goal FindGoal(string goalId)
    {
        if (allGoals == null) return null;
        foreach (Goal g in allGoals)
            if (g.goalId == goalId) return g;
        return null;
    }

    private GoalProgress FindOrCreateProgress(ProfileData profile, string goalId)
    {
        foreach (GoalProgress p in profile.goalProgress)
            if (p.goalId == goalId) return p;

        GoalProgress created = new GoalProgress { goalId = goalId, bestValue = 0, claimed = false };
        profile.goalProgress.Add(created);
        return created;
    }
}
