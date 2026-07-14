using UnityEngine;

public class ProfileUI : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform content;

    void Start()
    {
        for (int i = 0; i < ProfileManager.MaxProfileSlots; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, content);
            slotObj.GetComponent<ProfileSlot>().SetUp(i);
        }
    }
    
}
