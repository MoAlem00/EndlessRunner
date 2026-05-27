using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildGround : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private BasicObjectPooler groundPooler;
    [SerializeField] private BasicObjectPooler obstaclesPooler;
    [SerializeField] private int startingGroundAmount = 10;
    [SerializeField] private Vector3 gap;// = new Vector3(0, 0, 1f);
    private Vector3 startPos = new Vector3(0, 0, 0);
    
    private List<GameObject> groundObjects = new List<GameObject>();
    
    void Start()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        gap = groundPooler.GetPooledObjectScaleZ();
        BuildStartingGround();
    }

    private void Update()
    {
        if (player.position.z > groundObjects[1].transform.position.z + gap.z)
        {
            ReUseGround();
        }
    }

    private void SetNextPosition(Transform t)
    {
        startPos = t.position + gap;
    }
    private void BuildStartingGround()
    {
        for (int i = 0; i < startingGroundAmount; i++)
        {
            GameObject ground = groundPooler.GetPooledObject();
            ground.transform.position = startPos;
            SetNextPosition(ground.transform);
            groundObjects.Add(ground);
        }
    }

    public void ReUseGround()
    {
        GameObject firstGround = groundObjects[0];
        Ground currentGround = firstGround.GetComponent<Ground>();
        GameObject currentObstacle = currentGround.DetachObstacle();
        obstaclesPooler.ReturnObject(currentObstacle);
        firstGround.transform.position = startPos;
        groundObjects.RemoveAt(0);
        groundObjects.Add(firstGround);
        SetNextPosition(firstGround.transform);
        AttachNewObstacleTo(currentGround);
    }
    private void AttachNewObstacleTo(Ground ground)
    {
        GameObject newObstacle = obstaclesPooler.GetPooledObject();
        ground.AttachObstacle(newObstacle);
    }
}
