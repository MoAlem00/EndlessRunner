using UnityEngine;

public enum PowerUpType {ScoreMultiplier, Magnet, Invincibility}

[CreateAssetMenu(fileName = "PowerUps", menuName = "Scriptable Objects/PowerUps")]
public class PowerUps : ScriptableObject
{
    public GameObject prefab;
    public PowerUpType type;
    public float duration = 5f;
    public float spawnRate = 1f;
}
