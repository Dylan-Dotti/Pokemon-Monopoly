using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
            pView = GetComponent<PhotonView>();
            factory = GetComponent<PopupFactory>();
            manager = PopupManager.Instance;
        }
    }

    public void OpenPropertyMenu()
    {
        manager.QueuePopup(factory.PropertyMenu, PopupOpenOptions.Overlay);
    }

    public void OpenAuctionMenu(IEnumerable<PropertyData> properties,
        PopupOpenOptions openOptions = PopupOpenOptions.Overlay,
        bool allClients = false)
    {
        if (allClients)
        {
            var propNames = properties.Select(p => p.PropertyName).ToArray();
            pView.RPC("RPC_OpenAuctionMenu", RpcTarget.AllBuffered,
                propNames, openOptions);
            return;
        }
        AuctionMenu aMenu = factory.AuctionMenu;
        aMenu.SetAuctionData(properties);
        manager.QueuePopup(factory.AuctionMenu, openOptions);
    }

    public void OpenTextNotification(string text,
        PopupOpenOptions openOptions = PopupOpenOptions.Overlay,
        RpcTarget? rpcTarget = null)
    {
        if (rpcTarget.HasValue)
        {
            pView.RPC("RPC_OpenTextNotification", rpcTarget.Value,
                text, openOptions);
            return;
        }
        TextNotification notification = factory.GetTextNotification(text);
        manager.QueuePopup(notification, openOptions);
    }

    public void OpenImageNotification(string text, Sprite image,
        PopupOpenOptions openOptions = PopupOpenOptions.Overlay)
    {
        ImageNotification notification = factory.GetImageNotification(text, image);
        manager.QueuePopup(notification, openOptions);
    }

    public void OpenRentNotification(
        MonopolyPlayer payingPlayer, PropertyData rentedProperty,
        PopupOpenOptions openOptions = PopupOpenOptions.Overlay)
    {
        RentTransferNotification rentPopup = factory.GetRentNotification(
            payingPlayer, rentedProperty);
        manager.QueuePopup(rentPopup, openOptions);
    }

    public void OpenPropertyPurchasePrompt(
        MonopolyPlayer purchasingPlayer, PropertyData property,
        PopupOpenOptions openOptions = PopupOpenOptions.Overlay)
    {
        PropertyPurchasePrompt prompt = factory.GetPropertyPurchasePrompt(
            purchasingPlayer, property);
        manager.QueuePopup(prompt, openOptions);
    }

    public void OpenPropertyPurchasedNotification(
        MonopolyPlayer purchasingPlayer, PropertyData purchasedProperty,
        MonopolyPlayer purchasedFromPlayer = null,
        PopupOpenOptions openOptions = PopupOpenOptions.Overlay,
        bool allClients = false)
    {
        PropertyPurchasedNotification popup = factory.GetPropertyPurchasedNotification(
            purchasingPlayer, purchasedProperty, purchasedFromPlayer);
        manager.QueuePopup(popup, openOptions);
    }

    [PunRPC]
    private void RPC_OpenAuctionMenu(string[] propNames, PopupOpenOptions openOptions)
    {
        PropertyManager propManager = PropertyManager.Instance;
        IEnumerable<PropertyData> properties = propNames
            .Select(p => propManager.GetPropertyByName(p));
        OpenAuctionMenu(properties, openOptions);
    }

    [PunRPC]
    private void RPC_OpenTextNotification(string text, PopupOpenOptions openOptions)
    {
        OpenTextNotification(text, openOptions);
    }
}
