using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance;
    
    [SerializeField] private Theme defaultTheme;
    private ProfileData selectedProfile;
    public Theme selectedTheme;
    public int currentSeed;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if(selectedTheme == null)  selectedTheme = defaultTheme;
    }

    public void StartNewRun()
    {
        currentSeed = System.DateTime.Now.GetHashCode();
        if(selectedProfile != null)
            selectedProfile.lastRunSeed = currentSeed;
    }

    public void ReplayLastRun()
    {
        if(selectedProfile != null)
            currentSeed = selectedProfile.lastRunSeed;
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
