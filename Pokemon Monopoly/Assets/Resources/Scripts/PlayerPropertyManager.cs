using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(MonopolyPlayer))]
public class PlayerPropertyManager : MonoBehaviour
{
    private MonopolyPlayer player;
    private HashSet<PropertyData> properties;
    private PhotonView pView;

    public IReadOnlyCollection<PropertyData> Properties => properties;

    private void Awake()
    {
        player = GetComponent<MonopolyPlayer>();
        properties = new HashSet<PropertyData>();
        pView = GetComponent<PhotonView>();
    }

    public PropertyData GetPropertyByName(string propertyName) =>
        properties.Single(p => p.PropertyName == propertyName);

    /*[PunRPC]
    private void RPC_PurchaseProperty(
        string purchaserName, string propertyName, int? auctionPriceOverride)
    {
        PropertyData property = PropertyManager.Instance
            .GetPropertyByName(propertyName);
        properties.Add(property);
        property.Owner = this;
        if (auctionPriceOverride.HasValue)
        {
            Money -= auctionPriceOverride.Value;
            logger.LogEventLocal((IsLocalPlayer ? "You" : purchaserName) +
                $" won an auction for {property.PropertyName} " +
                $"with a bid of {auctionPriceOverride.Value.ToPokeMoneyString()}");
        }
        else
        {
            Money -= property.PurchaseCost;
            logger.LogEventLocal((IsLocalPlayer ? "You" : purchaserName) +
                $" purchased {property.PropertyName} for {property.PurchaseCost.ToPokeMoneyString()}");
            if (!IsLocalPlayer)
            {
                popupManager.QueuePopup(
                    popupManager.Factory.GetPropertyPurchasedNotification(this, property),
                    PopupOpenOptions.Queue, true);
            }
        }
    }

    [PunRPC]
    private void RPC_MortgageProperty(string playerName, string propertyName)
    {
        Debug.Log("Mortgaging Property: " + propertyName);
        PropertyData property = GetPropertyByName(propertyName);
        property.IsMortgaged = true;
        Money += property.MortgageValue;
        logger.LogEventLocal((IsLocalPlayer ? "You" : playerName) +
            $" mortgaged {propertyName} and received {property.MortgageValue.ToPokeMoneyString()} from the bank");
    }

    [PunRPC]
    private void RPC_UnmortgageProperty(string playerName, string propertyName)
    {
        Debug.Log("Unmortgaging Property" + propertyName);
        PropertyData property = GetPropertyByName(propertyName);
        property.IsMortgaged = false;
        Money -= property.UnmortgageCost;
        logger.LogEventLocal((IsLocalPlayer ?
            "You paid off your " : $"{playerName} paid off their ") +
            $"mortgage on {propertyName} for {property.UnmortgageCost.ToPokeMoneyString()}");
    }

    [PunRPC]
    private void RPC_PayRent(int toPlayerID, string propertyName)
    {
        MonopolyPlayer receivingPlayer = PlayerManager.Instance.GetPlayerByID(toPlayerID);
        Debug.Log("Paying rent to " + receivingPlayer.PlayerName);
        PropertyData property = receivingPlayer.GetPropertyByName(propertyName);
        Money -= property.CurrentRent;
        receivingPlayer.Money += property.CurrentRent;
        popupManager.QueueRentNotification(this, property);
    }

    [PunRPC]
    private void RPC_UpgradeProperty(string propertyName)
    {
        Debug.Log("Upgrading property: " + propertyName);
        GymPropertyData property = (GymPropertyData)GetPropertyByName(propertyName);
        Money -= property.UpgradeCost;
        property.Upgrade();
    }

    [PunRPC]
    private void RPC_DowngradeProperty(string propertyName)
    {
        Debug.Log("Downgrading property: " + propertyName);
        GymPropertyData property = (GymPropertyData)GetPropertyByName(propertyName);
        property.Downgrade();
        Money += property.DowngradeValue;
    }*/
}
