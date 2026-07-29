using TMPro;
using UnityEngine;
using UnityEngine.UI;

// How to use it:
// Attach this to the goal row prefab and assign the text, slider and button
// fields in the Inspector (progressBar, claimButton and claimedLabel are
// optional, safe to leave empty). ProgressionUI calls SetUp() once per goal,
// and the claim button's OnClick should call OnClaimPressed().
public class GoalSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Button claimButton;
    [SerializeField] private GameObject claimedLabel;

    private Goal goal;

    public void SetUp(Goal goalData)
    {
        goal = goalData;
        Refresh();
    }

    public void Refresh()
    {
        if (goal == null || RewardUnlockManager.Instance == null) return;

        GoalProgress progress = RewardUnlockManager.Instance.GetProgress(goal.goalId);
        int shown = Mathf.Min(progress.bestValue, goal.target);

        descriptionText.text = goal.description;
        progressText.text = $"{shown}/{goal.target}";

        if (progressBar != null)
        {
            progressBar.maxValue = goal.target;
            progressBar.value = shown;
        }

        bool complete = progress.bestValue >= goal.target;
        if (claimButton != null) claimButton.interactable = complete && !progress.claimed;
        if (claimedLabel != null) claimedLabel.SetActive(progress.claimed);
    }

    public void OnClaimPressed()
    {
        if (goal == null || RewardUnlockManager.Instance == null) return;
        if (RewardUnlockManager.Instance.TryClaimGoal(goal.goalId))
            Refresh();
    }
}
