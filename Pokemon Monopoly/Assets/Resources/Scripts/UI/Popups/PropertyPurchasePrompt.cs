﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyPurchasePrompt : Popup
{
    [SerializeField] private Image propertySprite;
    [SerializeField] private Text promptText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private Button viewPropertiesButton;

    private MonopolyPlayer purchasingPlayer;
    private PropertyData displayedProperty;

    public MonopolyPlayer PurchasingPlayer
    {
        get => purchasingPlayer;
        set
        {
            purchasingPlayer = value;
            UpdateButtons();
        }
    }

    public PropertyData DisplayedProperty
    {
        get => displayedProperty;
        set
        {
            displayedProperty = value;
            UpdatePrompt();
            UpdateSprite();
            UpdateButtons();
        }
    }

    private void Awake()
    {
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
        viewPropertiesButton.onClick.AddListener(OnViewPropertiesClicked);
    }

    public override void Close()
    {
        base.Close();
        Destroy(gameObject);
    }

    private void UpdatePrompt()
    {
        promptText.text =
            $"You landed on an unowned {displayedProperty.PropertyName}. " +
            $"Do you want to purchase {displayedProperty.PropertyName} for " +
            $"{SpecialStrings.POKEMONEY_SYMBOL}{displayedProperty.PurchaseCost}?";
    }

    private void UpdateSprite()
    {
        propertySprite.sprite = displayedProperty.PropertySprite;
    }

    private void UpdateButtons()
    {
        bool enable = purchasingPlayer != null && displayedProperty != null;
        yesButton.interactable = enable;
        noButton.interactable = enable;
        viewPropertiesButton.interactable = enable;
    }

    private void OnYesClicked()
    {
        Close();
        if (purchasingPlayer != null && displayedProperty != null)
        {
            purchasingPlayer.PurchaseProperty(displayedProperty);
        }
    }

    private void OnNoClicked()
    {
        Close();
        if (GameConfig.Instance.AuctionPropertyOnNoBuy)
        {
            PopupSpawner spawner = PopupSpawner.Instance;
            spawner.OpenAuctionMenu(
                new List<PropertyData> { displayedProperty },
                allClients: true);
            string suffix = $" did not purchase {displayedProperty.PropertyName}, " +
                "so it will be auctioned to the highest bidder.";
            spawner.OpenTextNotification("You" + suffix);
            spawner.OpenTextNotification(purchasingPlayer.PlayerName + suffix,
                rpcTarget: RpcTarget.OthersBuffered);
        }
    }

    private void OnViewPropertiesClicked()
    {
        PopupSpawner.Instance.OpenPropertyMenu();
    }
}
