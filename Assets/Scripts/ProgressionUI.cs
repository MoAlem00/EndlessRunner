using UnityEngine;

// How to use it:
// Attach this to the Progression panel and assign goalSlotPrefab and content
// in the Inspector. It builds one slot per goal the first time the panel is
// enabled, then just refreshes the existing slots every time after that.
public class ProgressionUI : MonoBehaviour
{
    [SerializeField] private GameObject goalSlotPrefab;
    [SerializeField] private Transform content;

    private GoalSlot[] spawnedSlots;

    private void OnEnable()
    {
        if (RewardUnlockManager.Instance == null) return;

        if (spawnedSlots == null)
            BuildSlots();
        else
            RefreshAll();
    }

    private void BuildSlots()
    {
        Goal[] goals = RewardUnlockManager.Instance.AllGoals;
        if (goals == null) return;

        spawnedSlots = new GoalSlot[goals.Length];
        for (int i = 0; i < goals.Length; i++)
        {
            GameObject slotObj = Instantiate(goalSlotPrefab, content);
            GoalSlot slot = slotObj.GetComponent<GoalSlot>();
            slot.SetUp(goals[i]);
            spawnedSlots[i] = slot;
        }
    }

    private void RefreshAll()
    {
        foreach (GoalSlot slot in spawnedSlots)
            if (slot != null) slot.Refresh();
    }
}
