using System.Collections.Generic;
using System.Data;
using UnityEngine;

public enum DifficultyType
{
    Easy = 0,
    Normal = 1,
    Hard = 2
}

[System.Serializable]
public struct DifficultyEntry
{
    public DifficultyType type;
    public Difficulty difficulty;
}

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private DifficultyEntry[] entries;

    private Dictionary<DifficultyType, Difficulty> map;
    public static DifficultyManager Instance { get; private set; }
    public int selectedDifficulty = -1;
    public Difficulty difficulty;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        map = new Dictionary<DifficultyType, Difficulty>();

        foreach (DifficultyEntry e in entries)
            map[e.type] = e.difficulty;

        if(selectedDifficulty == -1) SetDifficulty(DifficultyType.Normal);
        else SetDifficulty((DifficultyType)selectedDifficulty);
    }

    public void SetDifficulty(DifficultyType type) => ApplyDifficulty(map[type]);
    private void ApplyDifficulty(Difficulty difficulty)
    {
        this.difficulty = difficulty;
    }
}