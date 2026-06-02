using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Health Instance;
    [SerializeField] private int maxLives = 3;
    private int lives;
    public event Action<int> OnLivesChanged;
    public event Action OnDeath;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        lives = maxLives; 
        OnLivesChanged?.Invoke(lives);
    }

    public void LoseLife()
    {
        lives--;
        OnLivesChanged?.Invoke(lives);
        if (lives <= 0)
        {
            OnDeath?.Invoke();
        }
    }
}
