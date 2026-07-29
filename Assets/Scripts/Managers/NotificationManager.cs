using UnityEngine;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
#if UNITY_ANDROID
        RegisterChannel();
        new PermissionRequest();
#endif
    }
    
#if UNITY_ANDROID
    private void RegisterChannel()
    {
        var channel = new AndroidNotificationChannel
        {
            Id = "daily_reward",
            Name = "Daily Reward",
            Importance = Importance.Default,
            Description = "Daily reward reminders",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
    
    private void ScheduleDailyReminder()
    {
        AndroidNotificationCenter.CancelAllNotifications();

        var notification = new AndroidNotification
        {
            Title = "Daily Reward Ready!",
            Text = "Your daily reward is waiting. Come collect it!",
            FireTime = System.DateTime.Now.AddMinutes(1), // should be 24 hours!
            SmallIcon = "icon_0",
        };

        AndroidNotificationCenter.SendNotification(notification, "daily_reward");
    }
#endif
    private void OnApplicationPause(bool paused)
    {
#if UNITY_ANDROID
        if (paused) ScheduleDailyReminder();
#endif
    }
}

