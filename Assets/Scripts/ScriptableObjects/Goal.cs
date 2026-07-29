using UnityEngine;

public enum GoalType { CoinsInRun, DistanceInRun, PowerUpsInRun }

public enum RewardType { Coins, CoinMultiplierUpgrade, StartingShieldUpgrade, ThemeUnlock }

[CreateAssetMenu(fileName = "Goal", menuName = "Scriptable Objects/Goal")]
public class Goal : ScriptableObject
{
    public string goalId;
    public string description;
    public GoalType type;
    public int target;
    public int rewardCoins;

    [Header("Permanent Upgrade Reward")]
    public RewardType rewardType = RewardType.Coins;
    [Tooltip("Used when rewardType is CoinMultiplierUpgrade. 0.1 means +10% coins forever.")]
    public float upgradeAmount;
    [Tooltip("Used when rewardType is ThemeUnlock. Must match a Theme's themeName field.")]
    public string unlockThemeId;
}
