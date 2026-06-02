using System;
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
    public float startDistance;
}

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private DifficultyEntry[] entries;
    [SerializeField] private DifficultyType chosenType;
    
    private DifficultyType currentType;
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

        if(selectedDifficulty == -1) SetDifficulty(DifficultyType.Easy);
        else SetDifficulty((DifficultyType)selectedDifficulty);
    }

    private void Start()
    {
        currentType = chosenType;
    }

    private void Update()
    {
        float distance = DistanceTracker.Instance.GetDistance();
        DifficultyType best = chosenType;
        float bestDist = -1f;
        foreach (var entry in entries)
        {
            if (entry.startDistance <= distance && entry.startDistance > bestDist && (int)entry.type >= (int)chosenType)
            {
                best = entry.type;
                bestDist = entry.startDistance;
            }
        }
        if (best != currentType)
        {
            currentType = best;
            SetDifficulty(best);
        }
    }

    public void SetDifficulty(DifficultyType type) => ApplyDifficulty(map[type]);
    
    public void SelectDifficulty(int type) => chosenType = (DifficultyType)type;
    private void ApplyDifficulty(Difficulty difficulty)
    {
        this.difficulty = difficulty;
    }
}