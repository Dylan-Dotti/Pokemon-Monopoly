using UnityEngine;
using UnityEngine.UI;

public class TextNotification : Popup
{
    [SerializeField] protected Text notificationText;

    public string NotificationText
    {
        get => notificationText.text;
        set => notificationText.text = value;
    }
}
