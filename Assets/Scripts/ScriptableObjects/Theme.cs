using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Scriptable Objects/Theme")]
public class Theme : ScriptableObject
{
    public string themeName;
    public Sprite themeImage;
    public GameObject groundPrefab;
    public Material skybox;
    public GameObject[] obstaclesPrefab;
}
