using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text coinsEarnedText;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private Image thumbnailImage;

    private ProfileData data;

    public void SetUp(ProfileData profile)
    {
        data = profile;
        nameText.text = profile.profileName;
        scoreText.text = "Score: " + profile.highScore;
        distanceText.text = "Distance: " + profile.distance;
        coinsEarnedText.text = "Coins: " + profile.coinsEarned;
        thumbnailImage.sprite = LoadThumbnail(profile.thumbnailPath);
    }
    
    public void OnSelected()
    {
        ProfileManager.Instance.SelectProfile(data);
    }
    
    private Sprite LoadThumbnail(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path)) return null;

        byte[] bytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);

        return Sprite.Create(texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
    }
}
