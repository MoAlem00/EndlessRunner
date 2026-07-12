using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject themePanel;
    [SerializeField] private GameObject mainMenuPanel;

    private void Start()
    {
        StartCoroutine(AudioManager.Instance.PlayShuffleMusic());
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MoAlemScene");
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
}
