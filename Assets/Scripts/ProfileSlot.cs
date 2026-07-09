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
    }
    
    public void OnSelected()
    {
        ProfileManager.Instance.SelectProfile(data);
    }
}
