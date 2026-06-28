using System;
using UnityEngine;

public enum InputType
{
    Buttons = 0,
    Swipe = 1
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private InputType currentType;
    public InputType CurrentType => currentType;
    public event Action<InputType> OnTypeChange;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        currentType = (InputType)PlayerPrefs.GetInt("InputType");
        OnTypeChange?.Invoke(currentType);
    }

    public void SetInputType(int input)
    {
        currentType = (InputType)input;
        PlayerPrefs.SetInt("InputType", input);
        OnTypeChange?.Invoke(currentType);
    }
}