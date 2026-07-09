using System;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance;
    
    private ProfileData selectedProfile;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void SelectProfile(ProfileData data)
    {
        selectedProfile = data;
    }
}
