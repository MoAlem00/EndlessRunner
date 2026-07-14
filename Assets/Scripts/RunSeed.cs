using UnityEngine;

public class RunSeed : MonoBehaviour
{
    private void Awake()
    {
        int seed = ProfileManager.Instance.currentSeed;
        Random.InitState(seed);
        Debug.Log("Seed: " + seed);
    }
}
