using UnityEngine;

public class ThemeUI : MonoBehaviour
{
    [SerializeField] private GameObject themeSlotPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private Theme[] themes;

    void Start()
    {
        foreach (Theme theme in themes)
        {
            GameObject slot = Instantiate(themeSlotPrefab, content);
            slot.GetComponent<ThemeSlot>().SetUp(theme);
        }
    }
}
