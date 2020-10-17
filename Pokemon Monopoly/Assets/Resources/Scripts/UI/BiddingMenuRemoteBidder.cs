using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiddingMenuRemoteBidder : MonoBehaviour
{
    private RectTransform avatarImageHolder;
    private Text playerNameText;
    private PlayerMoneyText moneyText;

    private MonopolyPlayer linkedPlayer;

    public MonopolyPlayer LinkedPlayer
    {
        get => linkedPlayer;
        set
        {
            linkedPlayer = value;
            playerNameText.text = linkedPlayer.PlayerName;
            moneyText.LinkedPlayer = linkedPlayer;
            var avatarImage = linkedPlayer.GetNewAvatarImage(
                avatarImageHolder, Vector3.one * 0.3f);
        }
    }

    private void Awake()
    {
        avatarImageHolder = transform.Find("Avatar Image Holder").GetComponent<RectTransform>();
        playerNameText = transform.Find("Bidder Name").GetComponent<Text>();
        moneyText = transform.Find("Available Funds").GetComponent<PlayerMoneyText>();
    }
}
