using System;
using UnityEngine;
public class Collectable : MonoBehaviour
{
    [SerializeField] private int value = 100;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private PowerUp powerup; 
    [SerializeField] private float buffDuration = 10.0f;
    public static event Action<GameObject> OnPickedUp; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollect(other.gameObject);
        }
    }

    public void OnCollect(GameObject other)
    {

            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            
            int multiplier = 1;

            if(pc.HasEffect(PowerUpType.Multiplier)) multiplier++;
            Score.Instance.AddScore(value * multiplier);
            Score.Instance.CollectCoin(multiplier);
            AudioManager.Instance.PlaySfx(collectSound);
            OnPickedUp?.Invoke(gameObject);
            Score.Instance.AddScore(value);
            foreach(PowerUpType e in powerup.effects) pc.AddEffect(e, buffDuration);            
    }
}
