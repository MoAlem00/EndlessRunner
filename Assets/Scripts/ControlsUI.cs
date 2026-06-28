using UnityEngine;

public class ControlsUI : MonoBehaviour
{
    [SerializeField] private GameObject buttonsCanvas;

    void Start()
    {
        InputManager.Instance.OnTypeChange += UpdateButtons;
        UpdateButtons(InputManager.Instance.CurrentType);
    }
    void OnDestroy() => InputManager.Instance.OnTypeChange -= UpdateButtons;

    void UpdateButtons(InputType scheme)
    {
        buttonsCanvas.SetActive(scheme == InputType.Buttons);
    }
}
