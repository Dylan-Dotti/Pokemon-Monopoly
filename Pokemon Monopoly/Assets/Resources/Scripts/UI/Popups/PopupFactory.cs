using UnityEngine;

public class PopupFactory : MonoBehaviour
{
    [Header("Scene Menus")]
    [SerializeField] private PropertyMenu propertyMenu;
    [SerializeField] private AuctionMenu auctionMenu;

    [Header("Instantiated Popups")]
    [SerializeField] private TextNotification textNotificationPrefab;
    [SerializeField] private ImageNotification imageNotificationPrefab;
    [SerializeField] private PropertyPurchasePrompt purchasePromptPrefab;
    [SerializeField] private PropertyPurchasedNotification purchaseNotificationPrefab;
    [SerializeField] private RentTransferNotification rentNotificationPrefab;

    public PropertyMenu PropertyMenu => propertyMenu;
    public AuctionMenu AuctionMenu => auctionMenu;

    public TextNotification GetTextNotification(string text)
    {
        TextNotification notification = Instantiate(textNotificationPrefab);
        notification.NotificationText = text;
        return notification;
    }

    public ImageNotification GetImageNotification(string text, Sprite image)
    {
        ImageNotification notification = Instantiate(imageNotificationPrefab);
        notification.NotificationText = text;
        notification.NotificationImage = image;
        return notification;
    }

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
