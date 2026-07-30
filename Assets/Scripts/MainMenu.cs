using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject themePanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private GameObject progressionPanel;

    private void Start()
    {
        UpdateCoinsDisplay();
        if (AudioManager.Instance != null) StartCoroutine(AudioManager.Instance.PlayShuffleMusic());
        rewardPanel.SetActive(DailyRewardManager.Instance != null && DailyRewardManager.Instance.IsRewardAvailable());
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OpenTheme()
    {
        mainMenuPanel.SetActive(false);
        themePanel.SetActive(true);
    }

    public void CloseTheme()
    {
        mainMenuPanel.SetActive(true);
        themePanel.SetActive(false);
    }

    public void OnClaimPressed()
    {
        if (DailyRewardManager.Instance != null) DailyRewardManager.Instance.ClaimReward();
        rewardPanel.SetActive(false);
        UpdateCoinsDisplay();
    }

    public void OpenProgression()
    {
        mainMenuPanel.SetActive(false);
        progressionPanel.SetActive(true);
    }

    public void CloseProgression()
    {
        mainMenuPanel.SetActive(true);
        progressionPanel.SetActive(false);
        UpdateCoinsDisplay();
    }

    private void UpdateCoinsDisplay()
    {
        Debug.Log(ProfileManager.Instance);
        int coins = ProfileManager.Instance?.ActiveProfile != null
            ? ProfileManager.Instance.ActiveProfile.totalCoins
            : 0;
        coinsText.text = coins.ToString();
    }
}
