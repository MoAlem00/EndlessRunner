using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPosition;
    private GameObject currentObstacle;
    private GameObject currentCollectable;
    private GameObject currentPowerUp;
    private Vector3 collectableOffset = new Vector3(0f, 1f, 0f);
    private int spawnedIndex;

    
    public void AttachObstacle(GameObject obstacle)
    {
        if (obstacle == null || spawnPosition.Length == 0) return;
        currentObstacle = obstacle;
        int random = Random.Range(0, spawnPosition.Length);
        obstacle.transform.position = spawnPosition[random].position;
        spawnedIndex = random;
    }

    public GameObject DetachObstacle()
    {
        GameObject returnedObstacle = currentObstacle;
        currentObstacle = null;
        return returnedObstacle;
    }
    
    public void AttachCollectable(GameObject collectable)
    {
        if (collectable == null || currentCollectable != null) return;
        int offset = Random.Range(1, spawnPosition.Length);
        int random = (spawnedIndex + offset) % spawnPosition.Length;
        currentCollectable = collectable;
        collectable.transform.position = spawnPosition[random].position;
    }
    
    public GameObject DetachCollectable()
    {
        GameObject returnedCollectable = currentCollectable;
        currentCollectable = null;
        return returnedCollectable;
    }
    public bool TryClearCollectable(GameObject collectable)
    {
        if (currentCollectable != collectable) return false;
        currentCollectable = null;
        return true;
    }
    
    public bool TryClearObstacle(GameObject obstacle)
    {
        if (currentCollectable != obstacle) return false;
        currentObstacle = null;
        return true;
    }
    
    public void AttachPowerUp(GameObject powerUp)
    {
        if (powerUp == null || currentPowerUp != null) return;
        int offset = Random.Range(1, spawnPosition.Length);
        int random = (spawnedIndex + offset) % spawnPosition.Length;
        currentPowerUp = powerUp;
        powerUp.transform.position = spawnPosition[random].position;
    }
    
    public GameObject DetachPowerUp()
    {
        GameObject returnedPowerUp = currentPowerUp;
        currentPowerUp = null;
        return returnedPowerUp;
    }
    public bool TryClearPowerUp(GameObject powerUp)
    {
        if (currentPowerUp != powerUp) return false;
        currentPowerUp = null;
        return true;
    }
    
}
