using System;
using System.Collections;
using System.IO;
using UnityEngine;

[Serializable]
public class ProfileData
{
    public int slotIndex;
    public string profileName = "Player";
    public int highScore;
    public int totalCoins;
    public float bestDistance;
    public int chosenDifficulty;
    public int chosenInputType;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;
    public string lastSavedUtc;
    public int lastRunSeed;
    public string themeId;
}

// How to use it:
// Simply call ProfileManager.Instance.* where "*" is the functions  below:
//   - SaveProfile(slotIndex, name)      -> save + screenshot slot (call from a "Save" button).
//   - LoadProfile(slotIndex)            -> load + apply a slot; returns false if it's empty.
//   - DoesProfileExist(slotIndex)       -> check if it exists
//   - PeekProfile(slotIndex)            -> To preview data with applying it.
//   - LoadProfileScreenshot(slotIndex)  -> Texture2D thumbnail for a slot, or null if none saved yet.
//   - SetActiveSlot(slotIndex)          -> tell autosave-on-quit which slot to write to.
//   - OnProfileLoaded event            -> subscribe to refresh UI (e.g. ActiveProfile.highScore) after LoadProfile.
//   - Slots are indices 0..MaxProfileSlots-1.
public class ProfileManager : MonoBehaviour
{
    public const int MaxProfileSlots = 3;
    public Theme selectedTheme;
    [SerializeField] private Theme defaultTheme;
    [SerializeField] private Theme[] allThemes;
    public int currentSeed;
    public static ProfileManager Instance { get; private set; }
    public ProfileData ActiveProfile { get; private set; }
    public int ActiveSlotIndex { get; private set; }
    public event Action<ProfileData> OnProfileLoaded;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Directory.CreateDirectory(GetProfileFolder());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnApplicationQuit()
    {
        // Coroutines aren't guaranteed to run to completion once quit begins,
        // so autosave captures and writes synchronously instead of via SaveProfile().
        SaveProfileImmediate(ActiveSlotIndex);
    }
    
    public void SelectTheme(Theme theme) => selectedTheme = theme;
    
    private Theme FindThemeById(string id)
    {
        foreach (Theme t in allThemes)
            if (t.themeName == id) return t;
        return defaultTheme;
    }
    public void StartNewRun()
    {
        currentSeed = System.DateTime.Now.GetHashCode();
        if (ActiveProfile != null) ActiveProfile.lastRunSeed = currentSeed;
    }
    
    public void ReplayLastRun()
    {
        currentSeed = (ActiveProfile != null && ActiveProfile.lastRunSeed != 0)
            ? ActiveProfile.lastRunSeed
            : System.DateTime.Now.GetHashCode();
    }
    
    public static string GetProfileFolder() => Path.Combine(Application.persistentDataPath, "Profiles");
    private static string GetJsonPath(int slotIndex) => Path.Combine(GetProfileFolder(), $"profile_{slotIndex}.json");
    private static string GetScreenshotPath(int slotIndex) => Path.Combine(GetProfileFolder(), $"profile_{slotIndex}.png");

    /// Check if it exists
    public bool DoesProfileExist(int slotIndex) => File.Exists(GetJsonPath(slotIndex));

    public void SetActiveSlot(int slotIndex) => ActiveSlotIndex = slotIndex;

    /// For reading data and previewing it.
    public ProfileData PeekProfile(int slotIndex) => ReadProfileFromDisk(slotIndex);

    /// A screenshot of a profile slot
    public Texture2D LoadProfileScreenshot(int slotIndex)
    {
        string path = GetScreenshotPath(slotIndex);
        if (!File.Exists(path)) return null;

        byte[] pngBytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(pngBytes);
        return texture;
    }

    /// Call to save the profile, call SaveProfileImmediate if you want to force it now, important when quitting app.
    public void SaveProfile(int slotIndex, string profileName = null)
    {
        ActiveSlotIndex = slotIndex;
        StartCoroutine(SaveProfileRoutine(slotIndex, profileName));
    }

    private IEnumerator SaveProfileRoutine(int slotIndex, string profileName)
    {
        yield return new WaitForEndOfFrame();
        CaptureAndWriteScreenshot(slotIndex);
        WriteProfileJson(slotIndex, profileName);
    }

    public void SaveProfileImmediate(int slotIndex, string profileName = null)
    {
        CaptureAndWriteScreenshot(slotIndex);
        WriteProfileJson(slotIndex, profileName);
    }

    private void CaptureAndWriteScreenshot(int slotIndex)
    {
        Texture2D screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        try
        {
            byte[] pngBytes = screenshot.EncodeToPNG();
            File.WriteAllBytes(GetScreenshotPath(slotIndex), pngBytes);
        }
        catch (IOException e)
        {
            Debug.LogError($"[ProfileManager] Failed to save screenshot for slot {slotIndex}: {e.Message}");
        }
        finally
        {
            Destroy(screenshot);
        }
    }

    private void WriteProfileJson(int slotIndex, string profileName)
    {
        ProfileData existing = ReadProfileFromDisk(slotIndex);
        int currentScore = Score.Instance != null ? Score.Instance.CurrentScore : 0;
        int currentCoins = Score.Instance != null ? Score.Instance.CoinsCollected : 0;
        float currentDistance = DistanceTracker.Instance != null ? DistanceTracker.Instance.GetDistance() : 0f;

        ProfileData data = new ProfileData
        {
            slotIndex = slotIndex,
            profileName = !string.IsNullOrEmpty(profileName) ? profileName : (existing != null ? existing.profileName : "Player"),
            highScore = Mathf.Max(existing != null ? existing.highScore : 0, currentScore),
            totalCoins = (existing != null ? existing.totalCoins : 0) + currentCoins,
            bestDistance = Mathf.Max(existing != null ? existing.bestDistance : 0f, currentDistance),
            chosenDifficulty = PlayerPrefs.GetInt("ChosenDifficulty", 0),
            chosenInputType = PlayerPrefs.GetInt("InputType", 0),
            bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f),
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f),
            lastSavedUtc = DateTime.UtcNow.ToString("o"),
            lastRunSeed = currentSeed != 0 ? currentSeed : (existing != null ? existing.lastRunSeed : 0),
            themeId = selectedTheme != null ? selectedTheme.themeName : (existing != null ? existing.themeId : "")
        };

        try
        {
            Directory.CreateDirectory(GetProfileFolder());
            File.WriteAllText(GetJsonPath(slotIndex), JsonUtility.ToJson(data, true));
        }
        catch (IOException e)
        {
            Debug.LogError($"[ProfileManager] Failed to save profile {slotIndex}: {e.Message}");
        }
    }

    public bool LoadProfile(int slotIndex)
    {
        ProfileData data = ReadProfileFromDisk(slotIndex);
        if (data == null) return false;

        ActiveProfile = data;
        ActiveSlotIndex = slotIndex;

        PlayerPrefs.SetInt("ChosenDifficulty", data.chosenDifficulty);
        PlayerPrefs.SetInt("InputType", data.chosenInputType);
        PlayerPrefs.SetFloat("BGMVolume", data.bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", data.sfxVolume);

        if (DifficultyManager.Instance != null) DifficultyManager.Instance.SelectDifficulty(data.chosenDifficulty);
        if (InputManager.Instance != null) InputManager.Instance.SetInputType(data.chosenInputType);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(data.bgmVolume);
            AudioManager.Instance.SetSFXVolume(data.sfxVolume);
        }
        selectedTheme = FindThemeById(data.themeId);
        OnProfileLoaded?.Invoke(data);
        return true;
    }

    private ProfileData ReadProfileFromDisk(int slotIndex)
    {
        string path = GetJsonPath(slotIndex);
        if (!File.Exists(path)) return null;

        try
        {
            return JsonUtility.FromJson<ProfileData>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Debug.LogError($"[ProfileManager] Failed to read profile {slotIndex}: {e.Message}");
            return null;
        }
    }
}
