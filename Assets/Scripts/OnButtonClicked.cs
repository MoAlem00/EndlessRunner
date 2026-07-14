using UnityEngine;
using UnityEngine.SceneManagement;

public class OnButtonClicked : MonoBehaviour
{
    public void OnStartPressed()
    {
        //ProfileManager.Instance.StartNewRun();
        SceneManager.LoadScene("MoAlemScene");
    }

    public void OnReplayPressed()
    {
        //ProfileManager.Instance.ReplayLastRun();
        SceneManager.LoadScene("MoAlemScene");
    }
}
