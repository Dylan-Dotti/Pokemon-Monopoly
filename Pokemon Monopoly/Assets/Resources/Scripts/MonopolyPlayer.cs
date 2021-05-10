using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PlayerAvatarController))]
public class MonopolyPlayer : MonoBehaviour
{
    public static event UnityAction<MonopolyPlayer> Spawned;
    public event UnityAction<MonopolyPlayer> Despawned;
    public event UnityAction<MonopolyPlayer> EnteredJail;
    public event UnityAction<MonopolyPlayer> LeftJail;
    public event UnityAction<MonopolyPlayer> WentBankrupt;

    private PhotonView pView;
    private HashSet<PropertyData> properties;
    private int money;
    private string avatarImageName;
    private PopupSpawner popupSpawner;
    private AvatarImageFactory avatarFactory;
    private EventLogger logger;
    private PlayerUIManager playerUI;
    private PlayerAvatarController avatarController;

    public string PlayerName { get; private set; } = "Unknown";
    public int PlayerID { get; private set; }
    public bool HasAdditionalMove { get; private set; }
    public bool InJail { get; private set; }
    public int GetOutOfJailFreeUses { get; set; }
    public bool IsLocalPlayer => pView.IsMine;

    public PlayerAvatar Avatar => avatarController.Avatar;
    public IReadOnlyCollection<PropertyData> Properties => properties;

    public int Money
    {
        get => money;
        set
        {
            bool wentBelowZero = money >= 0 && value < 0;
            money = value;
            if (wentBelowZero) OnMoneyBelowZero();
        }
    }

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        money = GameConfig.Instance.PlayerStartingMoney;
        avatarController = GetComponent<PlayerAvatarController>();
        avatarController.CompletedAllActions += OnAllActionsCompleted;
        properties = new HashSet<PropertyData>();
        avatarFactory = AvatarImageFactory.Instance;
        playerUI = PlayerUIManager.Instance;
    }

    private void Start()
    {
        Debug.Log("Player start");
        logger = EventLogger.Instance;
        popupSpawner = PopupSpawner.Instance;
        if (IsLocalPlayer)
        {
            pView.RPC("RPC_SpawnInit", RpcTarget.AllBuffered,
                PhotonNetwork.LocalPlayer.NickName,
                MultiplayerSettings.Instance.AvatarImageName);
        }
    }

    private void OnDestroy()
    {
        DespawnAvatar();
        Despawned?.Invoke(this);
    }

    public void InitPlayerIdAllClients(int id)
    {
        pView.RPC("RPC_InitPlayerId", RpcTarget.AllBuffered, id);
    }

    // called only on local
    public void OnTurnStart()
    {
        playerUI.RollButtonInteractable = true;
        playerUI.LeaveJailInteractable = InJail;
        logger.LogEventLocal("Your turn has started");
        logger.LogEventOtherClients($"{PlayerName} started their turn");
    }

    // called only on local
    public void OnTurnEnd()
    {
        logger.LogEventLocal("You ended your turn");
        logger.LogEventOtherClients($"{PlayerName} ended their turn");
    }

    public void OnStandardRoll(DiceRoll roll)
    {
        if (InJail)
        {
            playerUI.EndTurnInteractable = true;
        }
        else
        {
            MoveAvatarSequentialAllClients(roll.RollTotal);
        }
    }

    public void OnEarnedAdditionalMove()
    {
        logger.LogEventLocal((IsLocalPlayer ? "You" : PlayerName) +
            " rolled doubles and earned an additional move.");
        if (IsLocalPlayer)
        {
            Debug.Log("Adding additional move");
            HasAdditionalMove = true;
        }
    }

    public void OnEnterJailWithDoubles()
    {
        popupSpawner.OpenTextNotification(
            $"{(IsLocalPlayer ? "You" : PlayerName)} rolled doubles 3 times in a row " +
            $"and {(IsLocalPlayer ? "were" : "was")} sent to jail.",
            PopupOpenOptions.Overlay);
        GoToJailAllClients();
    }

    public void OnExitJailWithDoubles()
    {
        popupSpawner.OpenTextNotification(
            $"{(IsLocalPlayer ? "You" : PlayerName)} rolled doubles " +
            "and escaped from jail.", PopupOpenOptions.Overlay);
        playerUI.LeaveJailInteractable = false;
        LeaveJailAllClients();
    }

    public void OnFailExitJailWithDoubles()
    {
        LeaveJailAllClients(true);
        playerUI.LeaveJailInteractable = false;
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
        avatarController.SpawnAvatar(this);
    }

    public void DespawnAvatar()
    {
        avatarController.DespawnAvatar();
    }

    public void MoveAvatarSequentialLocal(int numSquares, bool reversed = false)
    {
        avatarController.QueueSequentialMove(numSquares, reversed: reversed);
    }

    public void MoveAvatarSequentialAllClients(int numSquares, bool reversed = false)
    {
        pView.RPC("RPC_MoveAvatar", RpcTarget.AllBuffered,
            numSquares, reversed);
    }

    public void AddGetOutOfJailFreeUse()
    {
        Debug.Log("Adding get out of jail free use");
        GetOutOfJailFreeUses += 1;
    }

    public void GoToJailLocal()
    {
        if (!InJail)
        {
            InJail = true;
            avatarController.QueueLerpToJailSquare(hideDuringMove: true);
            EnteredJail?.Invoke(this);
        }
    }

    public void GoToJailAllClients()
    {
        pView.RPC("RPC_GoToJail", RpcTarget.AllBuffered);
    }

    public void LeaveJailAllClients(bool payFine = false)
    {
        if (payFine && GetOutOfJailFreeUses > 0)
        {
            GetOutOfJailFreeUses -= 1;
            payFine = false;
        }
        pView.RPC("RPC_LeaveJail", RpcTarget.AllBuffered, payFine);
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

    public void MortgagePropertyAllClients(PropertyData property)
    {
        if (property.Owner != this)
            throw new System.Exception("Can't mortgage an unowned property");
        pView.RPC("RPC_MortgageProperty", 
            RpcTarget.AllBuffered, PlayerName, property.PropertyName);
    }

    public void UnmortgagePropertyAllClients(PropertyData property)
    {
        if (property.Owner != this)
            throw new System.Exception("Can't mortgage an unowned property");
        else if (property.IsMortgaged == false)
            throw new System.Exception("Can't mortgage an unmortgaged property");
        pView.RPC("RPC_UnmortgageProperty",
            RpcTarget.AllBuffered, PlayerName, property.PropertyName);
    }

    public void PayRentAllClients(PropertyData property)
    {
        pView.RPC("RPC_PayRent", RpcTarget.AllBuffered,
            property.Owner.PlayerID, property.PropertyName);
    }

    public void UpgradePropertyAllClients(GymPropertyData gymProperty)
    {
        pView.RPC("RPC_UpgradeProperty", RpcTarget.AllBuffered,
            gymProperty.PropertyName);
    }

    public void DowngradePropertyAllClients(GymPropertyData gymProperty)
    {
        pView.RPC("RPC_DowngradeProperty", RpcTarget.AllBuffered,
            gymProperty.PropertyName);
    }

    private void OnAllActionsCompleted()
    {
        if (IsLocalPlayer)
        {
            playerUI.RollButtonInteractable = HasAdditionalMove;
            playerUI.EndTurnInteractable = !HasAdditionalMove;
            HasAdditionalMove = false;
        }
    }

    private void OnMoneyBelowZero()
    {
        Debug.Log("Money dropped below 0");
        int totalWorth = properties.Where(p => !p.IsMortgaged)
            .Select(p => p.MortgageValue + p.TotalDowngradeValue).Sum();
        Debug.Log("Total sellable/mortgagable worth: " + totalWorth);
        if (money + totalWorth < 0)
        {
            GoBankrupt();
        }
        else
        {
            if (IsLocalPlayer)
            {
                popupSpawner.OpenPropertyMenu();
                Debug.Log("You have enough funds to prevent bankruptcy");
            }
        }
    }

    private void GoBankrupt()
    {
        if (IsLocalPlayer)
        {
            Debug.Log("Going bankrupt");
            playerUI.DisableControlButtons();
            playerUI.ViewPropertiesInteractable = true;
        }
        logger.LogEventLocal(
            $"{(IsLocalPlayer ? "You" : PlayerName)} went bankrupt!");
        popupSpawner.OpenTextNotification(
            $"{(IsLocalPlayer ? "You" : PlayerName)} went bankrupt!");
        properties.ForEach(p => p.Owner = null);
        properties.Clear();
        avatarController.DespawnAvatar();
        WentBankrupt?.Invoke(this);
    }

    #region RPC Functions

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
        PlayerID = id;
        Debug.Log($"Player ID of {PlayerName} is now: {id}");
    }

    [PunRPC]
    private void RPC_MoveAvatar(int numSquares, bool reversed)
    {
        MoveAvatarSequentialLocal(numSquares, reversed: reversed);
    }

    [PunRPC]
    private void RPC_GoToJail()
    {
        GoToJailLocal();
    }

    [PunRPC]
    private void RPC_LeaveJail(bool payFine)
    {
        if (InJail)
        {
            InJail = false;
            avatarController.QueueLerpToJailSquare();
            if (payFine) Money -= 50;
            LeftJail?.Invoke(this);
        }
    }

    [PunRPC]
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
                popupSpawner.OpenPropertyPurchasedNotification(
                    this, property);
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
        popupSpawner.OpenRentNotification(this, property);
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

    #endregion
}
