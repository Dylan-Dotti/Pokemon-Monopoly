using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SimpleImageNotification : Popup
{
    private Image notificationImage;
    private Text notificationText;
    private Button closeButton;

    protected Sprite NotificationSprite
    {
        get => notificationImage.sprite;
        set => notificationImage.sprite = value;
    }

    protected string NotificationText
    {
        get => notificationText.text;
        set => notificationText.text = value;
    }

    protected bool CloseButtonInteractable
    {
        get => closeButton.interactable;
        set => closeButton.interactable = value;
    }

    protected virtual void Awake()
    {
        Transform panelContent = transform.Find("Panel Content");
        notificationImage = panelContent.Find("Notification Image").GetComponent<Image>();
        notificationText = panelContent.Find("Notification Text").GetComponent<Text>();
        closeButton = panelContent.Find("Close Button").GetComponent<Button>();
        closeButton.onClick.AddListener(Close);
    }
}
