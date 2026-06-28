using System.Collections.Generic;
using UnityEngine;

public class ArrayObjectPooler : MonoBehaviour
{
    [SerializeField] private BasicObjectPooler[] poolers;
    private Dictionary<GameObject, BasicObjectPooler> poolForPrefab;
    private Dictionary<GameObject, BasicObjectPooler> poolForActiveObject = new();

    void Awake()
    {
        poolForPrefab = new Dictionary<GameObject, BasicObjectPooler>();
        foreach (var p in poolers) 
            poolForPrefab[p.prefab] = p;
    }

    public GameObject Get(GameObject prefab)
    {
        var pooler = poolForPrefab[prefab];
        GameObject obj = pooler.GetPooledObject();
        poolForActiveObject[obj] = pooler;
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (obj == null) return;

        if (poolForActiveObject.TryGetValue(obj, out var pooler))
        {
            pooler.ReturnObject(obj);
            poolForActiveObject.Remove(obj);
        }
    }

    /*public GameObject GetWeightedRandom()
    {
        int totalWeight = 0;

        foreach (var pooler in poolers)
            totalWeight += pooler.initialPoolSize;

        int roll = Random.Range(0, totalWeight);

        foreach (var pooler in poolers)
        {
            roll -= pooler.initialPoolSize;

            if (roll < 0)
            {
                GameObject obj = pooler.GetPooledObject();
                poolForActiveObject[obj] = pooler;
                return obj;
            }
        }

        GameObject fallback = poolers[^1].GetPooledObject();
        poolForActiveObject[fallback] = poolers[^1];
        return fallback;
    }*/

    public PowerUp PickWeightedPowerUp()
    {
        var spawns = DifficultyManager.Instance.difficulty.powerUps;

        float total = 0f;
        foreach (var s in spawns) total += s.weight;

        float roll = Random.Range(0f, total);

        foreach (var s in spawns)
        {
            roll -= s.weight;
            if (roll <= 0f) return s.powerUp;
        }
        return spawns[spawns.Length - 1].powerUp;
    }
}
