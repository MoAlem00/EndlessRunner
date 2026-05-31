using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static event Action<GameObject> OnPickedUp; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPickedUp?.Invoke(gameObject);
            Debug.Log("Item Collected");
        }
    }
}
