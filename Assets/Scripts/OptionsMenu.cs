using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Image[] difficultyButtons; 
    private readonly Color selectedColor = Color.gray; 
    private readonly Color normalColor = Color.white;
    
    public void Close() => optionsPanel.SetActive(false);
    public void Open()
    {
        optionsPanel.SetActive(true);
        UpdateButtonVisuals(PlayerPrefs.GetInt("ChosenDifficulty", 0));
    }
    
    private void UpdateButtonVisuals(int selected)
    {
        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            difficultyButtons[i].color = (i == selected) ? selectedColor : normalColor;
        }
    }
    public void OnDifficultyButtonSelected(int type)
    {
        DifficultyManager.Instance.SelectDifficulty(type);
        UpdateButtonVisuals(type);
    }
    
}
