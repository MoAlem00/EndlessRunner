using System.Collections.Generic;
using UnityEngine;

public class ObstaclePoolManager : MonoBehaviour
{
    [SerializeField] private BasicObjectPooler[] obstaclePoolers;
    private Dictionary<GameObject, BasicObjectPooler> poolForPrefab;
    private Dictionary<GameObject, BasicObjectPooler> poolForActiveObject = new();

    void Awake()
    {
        poolForPrefab = new Dictionary<GameObject, BasicObjectPooler>();
        foreach (var p in obstaclePoolers) 
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
        poolForActiveObject[obj].ReturnObject(obj);
    }
}
