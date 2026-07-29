using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image themeImage;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private Button selectButton;

    private Theme theme;
    private bool isUnlocked = true;

    public void SetUp(Theme themeData)
    {
        theme = themeData;
        isUnlocked = RewardUnlockManager.Instance == null || RewardUnlockManager.Instance.IsThemeUnlocked(themeData.themeName);

        nameText.text = isUnlocked ? themeData.themeName : themeData.themeName + " (Locked)";
        themeImage.sprite = themeData.themeImage;

        if (lockIcon != null) lockIcon.SetActive(!isUnlocked);
        if (selectButton != null) selectButton.interactable = isUnlocked;
    }

    public void OnSelected()
    {
        Debug.Log(theme.themeName);
        if (ProfileManager.Instance == null) return;
        if (!isUnlocked) return;
        ProfileManager.Instance.SelectTheme(theme);
    }
}