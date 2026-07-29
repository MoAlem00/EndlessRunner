// UnityEngine also has a class called Event (the old IMGUI one), so alias
// this one to keep "Event" pointing at the Analytics type below.
using Event = Unity.Services.Analytics.Event;

// How to use it:
// This file is just the custom event definitions, one class per event.
// AnalyticsManager is what actually creates and sends them, so you only need
// to touch this file if you're adding a new event. If you add one, remember
// to also add a matching schema on the Unity Dashboard or it'll show up as
// invalid there.
public class SessionStartEvent : Event
{
    public SessionStartEvent() : base("session_start") { }
    public string AppVersion { set { SetParameter("app_version", value); } }
    public string Platform { set { SetParameter("platform", value); } }
}

public class RunStartedEvent : Event
{
    public RunStartedEvent() : base("run_started") { }
    public string Difficulty { set { SetParameter("difficulty", value); } }
    public int RunSeed { set { SetParameter("run_seed", value); } }
}

public class RunEndedEvent : Event
{
    public RunEndedEvent() : base("run_ended") { }
    public int Score { set { SetParameter("score", value); } }
    public int Coins { set { SetParameter("coins", value); } }
    public int Distance { set { SetParameter("distance_m", value); } }
}

public class MilestoneReachedEvent : Event
{
    public MilestoneReachedEvent() : base("milestone_reached") { }
    public int Milestone { set { SetParameter("distance_m", value); } }
}

public class RewardClaimedEvent : Event
{
    public RewardClaimedEvent() : base("reward_claimed") { }
    public string Source { set { SetParameter("source", value); } } // "daily" or "goal"
    public int Amount { set { SetParameter("amount", value); } }
}

public class GoalCompletedEvent : Event
{
    public GoalCompletedEvent() : base("goal_completed") { }
    public string GoalId { set { SetParameter("goal_id", value); } }
    public string RewardType { set { SetParameter("reward_type", value); } }
}
