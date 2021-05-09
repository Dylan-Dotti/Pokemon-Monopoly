using UnityEngine;
using UnityEngine.UI;

public class TextNotification : Popup
{
    [SerializeField] private Text notificationText;

    public string NotificationText
    {
        get => notificationText.text;
        set => notificationText.text = value;
    }
}
