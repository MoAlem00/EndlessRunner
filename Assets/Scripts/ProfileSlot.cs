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
    
    private int slotIndex;

    public void SetUp(int index)
    {
        slotIndex = index;
        ProfileData data = ProfileManager.Instance.PeekProfile(index);

        if (data == null)
        {
            nameText.text = "Empty Slot";
            scoreText.text = "No Data";
            distanceText.text = "No Data";
            coinsEarnedText.text = "No Data";
            thumbnailImage.enabled = false;
        }
        else
        {
            nameText.text = data.profileName;
            scoreText.text = "Score: " + data.highScore;
            distanceText.text = "Distance: " + Mathf.FloorToInt(data.bestDistance);
            coinsEarnedText.text = "Coins: " + data.totalCoins;
            thumbnailImage.enabled = true;
            Texture2D tex = ProfileManager.Instance.LoadProfileScreenshot(slotIndex);
            if (tex != null)
                thumbnailImage.sprite = Sprite.Create(tex,
                    new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
    }

    public void OnSelected()
    {
        ProfileManager.Instance.SetActiveSlot(slotIndex);

        if (ProfileManager.Instance.DoesProfileExist(slotIndex))
            ProfileManager.Instance.LoadProfile(slotIndex);
        else
        {
            ProfileManager.Instance.SaveProfileImmediate(slotIndex, "Player");
            ProfileManager.Instance.LoadProfile(slotIndex);
        }
    }
}
