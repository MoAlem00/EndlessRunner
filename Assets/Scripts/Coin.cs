using System;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private int value = 100;
    [SerializeField] private AudioClip collectSound;
    
    public static event Action<GameObject> OnPickedUp;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect(other.gameObject.GetComponent<PlayerController>());
        }
    }
    
    public void Collect(PlayerController player)
    {
        int multiplier = 1;
        if (player.HasEffect(PowerUpType.Multiplier)) multiplier++;
        Score.Instance.AddScore(value * multiplier);
        Score.Instance.CollectCoin(multiplier);
        AudioManager.Instance.PlaySfx(collectSound);
        OnPickedUp?.Invoke(gameObject);
    }
}
