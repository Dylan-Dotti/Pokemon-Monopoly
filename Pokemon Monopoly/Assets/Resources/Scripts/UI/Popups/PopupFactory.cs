using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupFactory : MonoBehaviour
{
    [Header("Scene Popups")]
    [SerializeField] private PropertyMenu propertyMenu;

    [Header("Instantiated Popups")]
    [SerializeField] private PropertyPurchasePrompt purchasePromptPrefab;
    [SerializeField] private PropertyPurchasedNotification purchaseNotificationPrefab;
    [SerializeField] private RentTransferNotification rentNotificationPrefab;

    public PropertyMenu GetPropertyMenu() => propertyMenu;

    public PropertyPurchasePrompt GetPropertyPurchasePrompt(
        MonopolyPlayer purchasingPlayer, PropertyData property)
    {
        PropertyPurchasePrompt newPrompt = Instantiate(purchasePromptPrefab);
        newPrompt.PurchasingPlayer = purchasingPlayer;
        newPrompt.DisplayedProperty = property;
        return newPrompt;
    }

    public PropertyPurchasedNotification GetPropertyPurchasedNotification(
        MonopolyPlayer purchasingPlayer, PropertyData purchasedProperty,
        MonopolyPlayer purchasedFromPlayer = null)
    {
        PropertyPurchasedNotification notification = Instantiate(purchaseNotificationPrefab);
        notification.PurchasingPlayer = purchasingPlayer;
        notification.PurchasedFromPlayer = purchasedFromPlayer;
        notification.PurchasedProperty = purchasedProperty;
        return notification;
    }

    public RentTransferNotification GetRentNotification(
        MonopolyPlayer payingPlayer, PropertyData rentedProperty)
    {
        RentTransferNotification notification = Instantiate(rentNotificationPrefab);
        notification.PayingPlayer = payingPlayer;
        notification.RentedProperty = rentedProperty;
        return notification;
    }
}
