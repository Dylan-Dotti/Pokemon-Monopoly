using UnityEngine;
using UnityEngine.UI;

public class ImageNotification : TextNotification
{
    [SerializeField] protected Image notificationImage;

    public Sprite NotificationImage
    {
        get => notificationImage.sprite;
        set => notificationImage.sprite = value;
    }
}
