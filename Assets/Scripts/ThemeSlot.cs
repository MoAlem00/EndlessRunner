using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private Image themeImage;

    private Theme theme;

    public void SetUp(Theme themeData)
    {
        theme = themeData;
        nameText.text = themeData.themeName;
        themeImage.sprite = themeData.themeImage;
    }

    public void OnSelected()
    {
        Debug.Log(theme.themeName);
        if(ProfileManager.Instance == null) return;
        ProfileManager.Instance.SelectTheme(theme);
    }
}