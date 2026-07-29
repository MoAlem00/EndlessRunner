using UnityEngine;

public enum GoalType { CoinsInRun, DistanceInRun, PowerUpsInRun }

[CreateAssetMenu(fileName = "Goal", menuName = "Scriptable Objects/Goal")]
public class Goal : ScriptableObject
{
    public string goalId;
    public string description;
    public GoalType type;
    public int target;
    public int rewardCoins; 
}
