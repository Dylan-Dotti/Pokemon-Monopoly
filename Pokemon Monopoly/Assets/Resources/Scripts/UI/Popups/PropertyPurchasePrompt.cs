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
            $"You landed on an unowned property: {displayedProperty.PropertyName}" +
            $"{System.Environment.NewLine}Do you want to purchase {displayedProperty.PropertyName} for " +
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
        if (purchasingPlayer != null && displayedProperty != null)
        {
            purchasingPlayer.PurchaseProperty(displayedProperty);
            Close();
        }
    }

    private void OnNoClicked()
    {
        if (GameConfig.Instance.AuctionPropertyOnNoBuy)
        {
            PopupManager.Instance.OverlayAuctionMenu(displayedProperty);
        }
        Close();
    }

    private void OnViewPropertiesClicked()
    {
        PopupManager.Instance.OverlayPropertyMenu();
    }
}
