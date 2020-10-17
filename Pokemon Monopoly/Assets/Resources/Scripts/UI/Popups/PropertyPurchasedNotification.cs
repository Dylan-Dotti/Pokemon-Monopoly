using UnityEngine;
using UnityEngine.UI;

public class PropertyPurchasedNotification : SimpleImageNotification
{
    private MonopolyPlayer purchasingPlayer;
    private MonopolyPlayer purchasedFromPlayer;
    private PropertyData purchasedProperty;

    public MonopolyPlayer PurchasingPlayer
    {
        get => purchasingPlayer;
        set
        {
            purchasingPlayer = value;
            UpdateDisplay();
        }
    }

    public MonopolyPlayer PurchasedFromPlayer
    {
        get => purchasedFromPlayer;
        set
        {
            purchasedFromPlayer = value;
            UpdateDisplay();
        }
    }

    public PropertyData PurchasedProperty
    {
        get => purchasedProperty;
        set
        {
            purchasedProperty = value;
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (purchasingPlayer != null && purchasedProperty != null)
        {
            NotificationSprite = purchasedProperty.PropertySprite;
            NotificationText = $"{purchasingPlayer.PlayerName} purchased " +
                $"{purchasedProperty.PropertyName} from the bank for " +
                $"{SpecialStrings.POKEMONEY_SYMBOL}{purchasedProperty.PurchaseCost}";
        }
    }
}
