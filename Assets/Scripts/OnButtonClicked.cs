using UnityEngine;
using UnityEngine.SceneManagement;

public class OnButtonClicked : MonoBehaviour
{
    public void OnStartPressed()
    {
        ProfileManager.Instance.StartNewRun();
        SceneManager.LoadScene("GameScene");
    }

    public void OnReplayPressed()
    {
        ProfileManager.Instance.ReplayLastRun();
        SceneManager.LoadScene("GameScene");
    }

    public void OnNextPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
