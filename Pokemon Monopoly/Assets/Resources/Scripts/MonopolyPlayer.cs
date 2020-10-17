﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class MonopolyPlayer : MonoBehaviour
{
    public static event UnityAction<MonopolyPlayer> Spawned;
    public event UnityAction<MonopolyPlayer> Despawned;

    [SerializeField] private PlayerAvatar avatarPrefab;

    private PhotonView pView;
    private MonopolyBoard board;
    private HashSet<PropertyData> properties;
    private int playerID;
    private string avatarImageName;

    private PopupManager popupManager;
    private AvatarImageFactory avatarFactory;
    private EventLogger logger;

    public string PlayerName { get; private set; } = "Unknown";
    public int Money { get; set; } = 1500;
    public bool InJail { get; set; }
    public IReadOnlyCollection<PropertyData> Properties => properties;
    public PlayerAvatar PlayerToken { get; set; }
    public PlayerManager Manager { get; set; }

    public int PlayerID
    {
        get => playerID;
        set
        {
            playerID = value;
            pView.RPC("RPC_InitPlayerId", RpcTarget.AllBuffered, value);
        }
    }

    public bool IsLocalPlayer => pView.IsMine;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        board = GameObject.FindGameObjectWithTag("Board")
            .GetComponent<MonopolyBoard>();
        properties = new HashSet<PropertyData>();
        avatarFactory = AvatarImageFactory.Instance;
    }

    private void Start()
    {
        popupManager = PopupManager.Instance;
        Debug.Log("Player start");
        logger = EventLogger.Instance;
        if (IsLocalPlayer)
        {
            pView.RPC("RPC_SpawnInit", RpcTarget.AllBuffered,
                PhotonNetwork.LocalPlayer.NickName,
                MultiplayerSettings.Instance.AvatarImageName);
        }
    }

    private void OnDestroy()
    {
        Despawned?.Invoke(this);
    }

    public PropertyData GetPropertyByName(string propertyName) =>
        properties.Single(p => p.PropertyName == propertyName);

    public RectTransform GetNewAvatarImage(
        Transform parent = null, Vector3? scale = null)
    {
        return avatarFactory.GetAvatarImage(avatarImageName, parent, scale);
    }

    public void SpawnAvatar()
    {
        Debug.Log("Spawning avatar");
        PlayerToken = Instantiate(avatarPrefab);
        PlayerToken.Owner = this;
        PlayerToken.SpawnAtSquare(board.GetGoSquare());
    }

    public void MoveAvatarLocal(int numSquares, MoveDirection direction)
    {
        IReadOnlyList<BoardSquare> squareSequence = board.GetNextSquares(
            PlayerToken.OccupiedSquare, numSquares, 
            direction == MoveDirection.Backward);
        StartCoroutine(MoveAvatarCR(squareSequence, 0.5f));
    }

    public void MoveAvatarAllClients(int numSquares, MoveDirection direction)
    {
        pView.RPC("RPC_MoveAvatar", RpcTarget.AllBufferedViaServer,
            numSquares, direction);
    }

    public void GoToJail(BoardSquare jailSquare)
    {
        InJail = true;
        PlayerToken.MoveToSquare(jailSquare, true);
    }

    public void PurchaseProperty(PropertyData property)
    {
        pView.RPC("RPC_PurchaseProperty",
            RpcTarget.AllBuffered, PlayerName, property.PropertyName, null);
    }

    public void PurchasePropertyFromAuction(PropertyData property, int bid)
    {
        pView.RPC("RPC_PurchaseProperty",
            RpcTarget.AllBuffered, PlayerName, property.PropertyName, bid);
    }

    public void MortgageProperty(PropertyData property)
    {
        if (property.Owner != this)
            throw new System.Exception("Can't mortgage an unowned property");
        pView.RPC("RPC_MortgageProperty", 
            RpcTarget.AllBuffered, PlayerName, property.PropertyName);
    }

    public void UnmortgageProperty(PropertyData property)
    {
        if (property.Owner != this)
            throw new System.Exception("Can't mortgage an unowned property");
        else if (property.IsMortgaged == false)
            throw new System.Exception("Can't mortgage an unmortgaged property");
        pView.RPC("RPC_UnmortgageProperty",
            RpcTarget.AllBuffered, PlayerName, property.PropertyName);
    }

    public void PayRent(PropertyData property)
    {
        pView.RPC("RPC_PayRent", RpcTarget.AllBuffered,
            property.Owner.PlayerID, property.PropertyName);
    }

    public void UpgradeProperty(GymPropertyData gymProperty)
    {
        pView.RPC("RPC_UpgradeProperty", RpcTarget.AllBuffered,
            gymProperty.PropertyName);
    }

    public void DowngradeProperty(GymPropertyData gymProperty)
    {
        pView.RPC("RPC_DowngradeProperty", RpcTarget.AllBuffered,
            gymProperty.PropertyName);
    }

    private IEnumerator MoveAvatarCR(
        IReadOnlyList<BoardSquare> squareSequence, float interval)
    {
        for (int i = 0; i < squareSequence.Count - 1; i++)
        {
            yield return new WaitForSeconds(interval);
            PlayerToken.MoveToSquare(squareSequence[i]);
        }
        yield return new WaitForSeconds(interval);
        PlayerToken.MoveToSquare(
            squareSequence[squareSequence.Count - 1], isLastMove: true);
    }

    [PunRPC]
    private void RPC_SpawnInit(string name, string avatarImageName)
    {
        PlayerName = name;
        this.avatarImageName = avatarImageName;
        Spawned?.Invoke(this);
    }
    
    [PunRPC]
    private void RPC_InitPlayerId(int id)
    {
        playerID = id;
        Debug.Log($"Player ID of {PlayerName} is now: {id}");
    }

    [PunRPC]
    private void RPC_MoveAvatar(int numSquares, MoveDirection direction)
    {
        MoveAvatarLocal(numSquares, direction);
    }

    [PunRPC]
    private void RPC_PurchaseProperty(string purchaserName, string propertyName,
        int? auctionPriceOverride)
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
        MonopolyPlayer receivingPlayer = Manager.GetPlayerByID(toPlayerID);
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
    }
}
