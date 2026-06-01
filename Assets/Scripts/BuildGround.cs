using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildGround : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private ObstaclePoolManager obstaclePoolerManager;
    [SerializeField] private BasicObjectPooler groundPooler;
    //[SerializeField] private BasicObjectPooler obstaclesPooler;
    [SerializeField] private BasicObjectPooler collectablesPooler;
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
            Ground currentGround = ground.GetComponent<Ground>();
            ground.transform.position = startPos;
            SetNextPosition(ground.transform);
            groundObjects.Add(ground);
            AttachNewObstacleTo(currentGround);
            AttachNewCollectableTo(currentGround);
        }
    }

    public void ReUseGround()
    {
        GameObject firstGround = groundObjects[0];
        Ground currentGround = firstGround.GetComponent<Ground>();
        GameObject currentObstacle = currentGround.DetachObstacle();
        GameObject currentCollectable = currentGround.DetachCollectable();
        collectablesPooler.ReturnObject(currentCollectable);
        obstaclePoolerManager.Return(currentObstacle);
        firstGround.transform.position = startPos;
        groundObjects.RemoveAt(0);
        groundObjects.Add(firstGround);
        SetNextPosition(firstGround.transform);
        if (Random.value < DifficultyManager.Instance.difficulty.spawnRate)
            AttachNewObstacleTo(currentGround);
        AttachNewCollectableTo(currentGround);
    }
    private void AttachNewObstacleTo(Ground ground)
    {
        var list = DifficultyManager.Instance.difficulty.ObstacleTypes;
        GameObject chosen = list[Random.Range(0, list.Length)];
        GameObject newObstacle = obstaclePoolerManager.Get(chosen);
        ground.AttachObstacle(newObstacle);
    }
    
    private void AttachNewCollectableTo(Ground ground)
    {
        GameObject newCollectable = collectablesPooler.GetPooledObject();
        ground.AttachCollectable(newCollectable);
    }
    
    private void OnEnable()
    {
        Collectable.OnPickedUp += HandleCollectablePickedUp;
    }

    private void OnDisable()
    {
        Collectable.OnPickedUp -= HandleCollectablePickedUp;
    }
    private void HandleCollectablePickedUp(GameObject collectable)
    {
        foreach (GameObject ground in groundObjects)
        {
            Ground currGround = ground.GetComponent<Ground>();
            if (currGround.TryClearCollectable(collectable)) break;
        }
        collectablesPooler.ReturnObject(collectable);
    }
    
}
