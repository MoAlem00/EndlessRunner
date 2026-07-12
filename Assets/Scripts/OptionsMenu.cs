using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Image[] difficultyButtons;
    [SerializeField] private Image[] InputButtons;
    private readonly Color selectedColor = Color.gray; 
    private readonly Color normalColor = Color.white;
    
    public void Close() => optionsPanel.SetActive(false);
    
    public void OnMusicSliderChanged(float value) => AudioManager.Instance.SetBGMVolume(value);
    public void OnSfxSliderChanged(float value)   => AudioManager.Instance.SetSFXVolume(value);
    public void Open()
    {
        optionsPanel.SetActive(true);
        UpdateButtonVisuals(PlayerPrefs.GetInt("ChosenDifficulty", 0));
        UpdateInputVisuals(PlayerPrefs.GetInt("InputType", 0));
    }
    
    private void UpdateButtonVisuals(int selected)
    {
        for (int i = 0; i < difficultyButtons.Length; i++)
        {
            difficultyButtons[i].color = (i == selected) ? selectedColor : normalColor;
        }
    }
    
    private void UpdateInputVisuals(int selected)
    {
        for (int i = 0; i < InputButtons.Length; i++)
            InputButtons[i].color = (i == selected) ? selectedColor : normalColor;
    }
    public void OnDifficultyButtonSelected(int type)
    {
        DifficultyManager.Instance.SelectDifficulty(type);
        UpdateButtonVisuals(type);
    }
    
}
