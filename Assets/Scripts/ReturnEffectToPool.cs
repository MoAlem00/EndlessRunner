using UnityEngine;

public class ReturnEffectToPool : MonoBehaviour
{
    public BasicObjectPooler pool;
    private ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    } 

    void OnEnable()
    {
        Invoke(nameof(ReturnToPool), ps.main.duration);
    }
    void OnDisable()
    {
        CancelInvoke();
    }

    void ReturnToPool()
    { 
        if(pool != null)
            pool.ReturnObject(gameObject);
    }
}
