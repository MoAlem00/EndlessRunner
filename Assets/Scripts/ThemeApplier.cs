using UnityEngine;

public class ThemeApplier : MonoBehaviour
{
    private void Start()
    {
        Theme theme = ProfileManager.Instance.selectedTheme;
        if (theme == null || theme.skybox == null) return;
        
        RenderSettings.skybox = theme.skybox;
    }
}
