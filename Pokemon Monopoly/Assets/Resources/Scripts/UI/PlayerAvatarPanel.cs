using UnityEngine;

public class PlayerAvatarPanel : MonoBehaviour
{
    private PlayerNameText pNameText;
    private PlayerMoneyText pMoneyText;
    private Transform imageHolder;

    public MonopolyPlayer LinkedPlayer
    {
        get => pNameText.LinkedPlayer;
        set
        {
            pNameText.LinkedPlayer = value;
            pMoneyText.LinkedPlayer = value;
            if (value != null) SetAvatarImage();
        }
    }

    private void Awake()
    {
        pNameText = GetComponentInChildren<PlayerNameText>();
        pMoneyText = GetComponentInChildren<PlayerMoneyText>();
        imageHolder = transform.Find("Avatar Image Holder");
    }

    private void SetAvatarImage()
    {
        if (imageHolder.transform.childCount > 0)
        {
            Destroy(imageHolder.transform.GetChild(0).gameObject);
        }
        LinkedPlayer.GetNewAvatarImage(parent: imageHolder);
    }
}
