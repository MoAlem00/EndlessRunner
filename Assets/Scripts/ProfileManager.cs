using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance;
    
    private ProfileData selectedProfile;
    public Theme selectedTheme;
    
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
        Debug.Log(data.profileName);
    }

    public void SelectTheme(Theme theme)
    {
        selectedTheme = theme;
        Debug.Log(theme.name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
