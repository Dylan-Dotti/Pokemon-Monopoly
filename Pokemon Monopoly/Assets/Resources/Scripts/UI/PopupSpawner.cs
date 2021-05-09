using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupSpawner : MonoBehaviour
{
    public static PopupSpawner Instance { get; private set; }

    private PopupFactory factory;
    private PopupManager manager;
    private PhotonView pView;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            factory = GetComponent<PopupFactory>();
            manager = PopupManager.Instance;
        }
    }

    public void OverlayPropertyMenu()
    {
        manager.QueuePopup(factory.PropertyMenu, PopupOpenOptions.Overlay);
    }

    public void OverlayAuctionMenu(IEnumerable<PropertyData> properties)
    {
        AuctionMenu aMenu = factory.AuctionMenu;
        aMenu.SetAuctionData(properties);
        manager.QueuePopup(factory.AuctionMenu, PopupOpenOptions.Overlay);
    }

    public void OverlayAuctionMenuAllClients(IEnumerable<PropertyData> properties)
    {
        var propNames = properties.Select(p => p.PropertyName).ToArray();
        pView.RPC("RPC_OpenAuctionMenu", RpcTarget.AllBuffered, (object)propNames);
    }

    public TextNotification QueueTextNotification(string text)
    {
        TextNotification notification = factory.GetTextNotification(text);
        manager.QueuePopup(notification, PopupOpenOptions.Queue);
        return notification;
    }

    public RentTransferNotification QueueRentNotification(
        MonopolyPlayer payingPlayer, PropertyData rentedProperty)
    {
        RentTransferNotification rentPopup = factory.GetRentNotification(
            payingPlayer, rentedProperty);
        manager.QueuePopup(rentPopup, PopupOpenOptions.Queue);
        return rentPopup;
    }

    public PropertyPurchasePrompt QueuePropertyPurchasePrompt(
        MonopolyPlayer purchasingPlayer, PropertyData property)
    {
        PropertyPurchasePrompt prompt = factory.GetPropertyPurchasePrompt(
            purchasingPlayer, property);
        manager.QueuePopup(prompt, PopupOpenOptions.Queue);
        return prompt;
    }

    public PropertyPurchasedNotification QueuePropertyPurchasedNotification(
        MonopolyPlayer purchasingPlayer, PropertyData purchasedProperty,
        MonopolyPlayer purchasedFromPlayer = null)
    {
        PropertyPurchasedNotification popup = factory.GetPropertyPurchasedNotification(
            purchasingPlayer, purchasedProperty, purchasedFromPlayer);
        manager.QueuePopup(popup, PopupOpenOptions.Queue, true);
        return popup;
    }

    [PunRPC]
    private void RPC_OpenAuctionMenu(string[] propNames)
    {
        PropertyManager propManager = PropertyManager.Instance;
        IEnumerable<PropertyData> properties = propNames
            .Select(p => propManager.GetPropertyByName(p));
        OverlayAuctionMenu(properties);

    }
}
