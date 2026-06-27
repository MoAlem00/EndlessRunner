using System;
using UnityEngine;
using SE = PlayerController.StatusEffectType;
public class Collectable : MonoBehaviour
{
    [SerializeField] private int value = 100;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private SE[] effects;
    [SerializeField] private float buffDuration = 10.0f;
    public static event Action<GameObject> OnPickedUp; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Score.Instance.AddScore(value);
            Score.Instance.CollectCoin();
            AudioManager.Instance.PlaySfx(collectSound);
            OnPickedUp?.Invoke(gameObject);
            Score.Instance.AddScore(value);
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            foreach(SE e in effects) pc.AddEffect(e, buffDuration);            
        }
    }
}
