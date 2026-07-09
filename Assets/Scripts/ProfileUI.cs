using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProfileUI : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform content;


    void Start()
    {
        List<ProfileData> profiles = GetProfiles();
        Populate(profiles);
    }

    private void Populate(List<ProfileData> profiles)
    {
        foreach (ProfileData profile in profiles)
        {
            GameObject slotObj = Instantiate(slotPrefab, content);
            slotObj.GetComponent<ProfileSlot>().SetUp(profile);
        }
    }

    private List<ProfileData> GetProfiles()
    {
        return new List<ProfileData>
        {
            new ProfileData { profileName = "Player 1", highScore = 500, distance = 1200, coinsEarned = 30 },
            new ProfileData { profileName = "Player 2", highScore = 900, distance = 3000, coinsEarned = 75 },
            new ProfileData { profileName = "Player 3", highScore = 15000, distance = 7000, coinsEarned = 350 },
            new ProfileData { profileName = "Mohammad", highScore = 500000, distance = 324324, coinsEarned = 87555 },
            new ProfileData { profileName = "Sami", highScore = 345435, distance = 23456, coinsEarned = 38999 },
        };
    }
}
