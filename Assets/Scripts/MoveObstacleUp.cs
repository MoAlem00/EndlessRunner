using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveObstacleUp : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minY = 1f;
    [SerializeField] private float maxY = 5f;
    private float maxSpeed = 3f;
    private float minSpeed = 1f;
    private Vector3 direction;

    private void Start()
    {
        int randomDir = Random.Range(0, 2);
        if (randomDir == 0) direction = Vector3.up;
        else if (randomDir == 1) direction = Vector3.down;
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        speed = randomSpeed;
    }

    void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
        if (transform.position.y >= maxY)      { SetY(maxY); direction = Vector3.down; }
        else if (transform.position.y <= minY)  { SetY(minY); direction = Vector3.up; }
    }

    void SetY(float y)
    {
        Vector3 p = transform.position;
        p.y = y;
        transform.position = p;
    }

}
