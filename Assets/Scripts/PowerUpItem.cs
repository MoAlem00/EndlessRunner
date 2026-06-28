using System;
using UnityEngine;

public class PowerUpItem : MonoBehaviour, ICollectable
{
    [SerializeField] private int value = 100;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private PowerUp powerUp; 
    //[SerializeField] private float buffDuration = 10.0f;
    
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
        AudioManager.Instance.PlaySfx(collectSound);
        foreach(PowerUpType e in powerUp.effects) player.AddEffect(e, powerUp.duration);
        OnPickedUp?.Invoke(gameObject);
    }
}
