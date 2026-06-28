using UnityEngine;

[System.Serializable]
public struct PowerUpSpawn
{
    public PowerUps powerUp;
    public float weight;
}

[CreateAssetMenu(fileName = "Difficulty", menuName = "Scriptable Objects/Difficulty")]
public class Difficulty : ScriptableObject
{
    [Tooltip("Rate of which objects spawn")]
    public float spawnRate = 1;
    [Tooltip("Multiplies the speed of the player")]
    public float movementSpeedMultiplier = 1;
    [Tooltip("The maximum amount of obstacles allowed at a time.")]
    public int maxObstacles = 20;
    [Tooltip("Obstacle Types Per Difficulty")]
    public GameObject[] ObstacleTypes;
    [Tooltip("PowerUps")]
    public float powerUpChance;
    public PowerUpSpawn[] powerUps;
}
