using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuildGround : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private ObstaclePoolManager obstaclePoolerManager;
    [SerializeField] private BasicObjectPooler groundPooler;
    [SerializeField] private BasicObjectPooler coinsPooler;
    [SerializeField] private ArrayObjectPooler powerUpsPooler;
    [SerializeField] private int startingGroundAmount = 5;
    [SerializeField] private Vector3 gap;
    private Vector3 startPos = new Vector3(0, 0, 0);
    private List<GameObject> groundObjects = new List<GameObject>();
    
    void Start()
    {
        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        gap = groundPooler.GetPooledObjectScaleZ();
        Debug.Log(gap);
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
            if (i > startingGroundAmount / 2)
            {
                AttachNewObstacleTo(currentGround);
                AttachNewCollectableTo(currentGround);
            }
        }
    }

    public void ReUseGround()
    {
        GameObject firstGround = groundObjects[0];
        Ground currentGround = firstGround.GetComponent<Ground>();
        GameObject currentObstacle = currentGround.DetachObstacle();
        GameObject currentCollectable = currentGround.DetachCollectable();
        GameObject currentPowerUp = currentGround.DetachPowerUp();
        coinsPooler.ReturnObject(currentCollectable);
        obstaclePoolerManager.Return(currentObstacle);
        powerUpsPooler.Return(currentPowerUp);
        firstGround.transform.position = startPos;
        groundObjects.RemoveAt(0);
        groundObjects.Add(firstGround);
        SetNextPosition(firstGround.transform);
        if (Random.value <= DifficultyManager.Instance.difficulty.spawnRate)
            AttachNewObstacleTo(currentGround);
        if (Random.value <= DifficultyManager.Instance.difficulty.powerUpChance)
            AttachNewPowerUpTo(currentGround);
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
        GameObject newCollectable = coinsPooler.GetPooledObject();
        ground.AttachCollectable(newCollectable);
    }
    private void AttachNewPowerUpTo(Ground ground)
    {
        PowerUp chosen = powerUpsPooler.PickWeightedPowerUp();
        GameObject newPowerUp = powerUpsPooler.Get(chosen.prefab);
        ground.AttachPowerUp(newPowerUp);
    }
    
    private void OnEnable()
    {
        Coin.OnPickedUp += HandleCollectablePickedUp;
        PowerUpItem.OnPickedUp += HandleCollectablePickedUp;
        PlayerController.OnHitObstacle += HandleObstacleHit;
    }

    private void OnDisable()
    {
        Coin.OnPickedUp -= HandleCollectablePickedUp;
        PowerUpItem.OnPickedUp -= HandleCollectablePickedUp;
        PlayerController.OnHitObstacle -= HandleObstacleHit;
    }
    private void HandleCollectablePickedUp(GameObject collectable)
    {
        foreach (GameObject ground in groundObjects)
        {
            Ground currGround = ground.GetComponent<Ground>();
            if (currGround.TryClearCollectable(collectable))
            {
                coinsPooler.ReturnObject(collectable);
                return;
            }

            if (currGround.TryClearPowerUp(collectable))
            {
                powerUpsPooler.Return(collectable);
                return;
            }
        }
    }

    private void HandleObstacleHit(GameObject obstacle)
    {
        foreach (GameObject ground in groundObjects)
        {
            Ground currGround = ground.GetComponent<Ground>();
            if (currGround.TryClearObstacle(obstacle)) break;
        }
        obstaclePoolerManager.Return(obstacle);
    }
    
}
