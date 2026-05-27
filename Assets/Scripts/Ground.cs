using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private Transform[] obstaclesPosition;
    private GameObject currentObstacle;

    
    public void AttachObstacle(GameObject obstacle)
    {
        if (obstacle == null || obstaclesPosition.Length == 0) return;
        currentObstacle = obstacle;
        int random = Random.Range(0, obstaclesPosition.Length);
        obstacle.transform.position = obstaclesPosition[random].position;
    }

    public GameObject DetachObstacle()
    {
        GameObject returnedObstacle = currentObstacle;
        currentObstacle = null;
        return returnedObstacle;
    }
}
