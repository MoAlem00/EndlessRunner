using System;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public static DistanceTracker Instance;
    
    private float startZ;
    private float distance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        startZ = transform.position.z;
    }

    public float GetDistance()
    {
        return distance = transform.position.z - startZ;
    }

    public float ResetDistance()
    {
        return distance = 0;
    }
}
