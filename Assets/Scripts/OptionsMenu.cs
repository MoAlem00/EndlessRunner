using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    
    public void Open() => optionsPanel.SetActive(true);
    public void Close() => optionsPanel.SetActive(false);
    
}
