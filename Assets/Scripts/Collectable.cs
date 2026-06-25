using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int value = 100;
    [SerializeField] private AudioClip collectSound;
    public static event Action<GameObject> OnPickedUp; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Score.Instance.AddScore(value);
            Score.Instance.CollectCoin();
            AudioManager.Instance.PlaySfx(collectSound);
            OnPickedUp?.Invoke(gameObject);
            Debug.Log("Item Collected");
        }
    }
}
