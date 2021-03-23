﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class AuctionMenu : Popup
{
    [SerializeField] private PropertyDetailsDisplay propertyDisplay;
    [SerializeField] private BackNextMenuControl backNextControl;
    [SerializeField] private Text auctioningWhenText;
    [SerializeField] private MultiplayerMessageLog messageLog;

    private PhotonView pView;
    private PlayerManager playerManager;
    private BiddingMenu bidMenu;
    private Bid highestBid;

    private List<PropertyData> propertiesToAuction;

    public PropertyData CurrentAuctioningProperty => propertiesToAuction[0];

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        bidMenu = GetComponentInChildren<BiddingMenu>();
        bidMenu.BidSubmitted += OnBidSubmitted;
        bidMenu.AuctionComplete += OnAuctionComplete;
        backNextControl.Back += OnPropertyIndexChanged;
        backNextControl.Next += OnPropertyIndexChanged;
    }

    private void Start()
    {
        playerManager = PlayerManager.Instance;
        playerManager.PlayersReady += OnPlayersReady;
        if (playerManager.LocalPlayer != null && playerManager.RemotePlayers != null)
        {
            bidMenu.LocalPlayer = playerManager.LocalPlayer;
            bidMenu.RemotePlayers = new List<MonopolyPlayer>(playerManager.RemotePlayers);
        }
        PropertyData[] properties = new PropertyData[] 
        {
            PropertyManager.Instance.GetPropertyByName("Growlithe"),
            PropertyManager.Instance.GetPropertyByName("Ponyta"),
            PropertyManager.Instance.GetPropertyByName("Rapidash")
        };
        StartNewAuction(properties);
    }

    public void StartNewAuction(params PropertyData[] propsToAuction)
    {
        propertiesToAuction = new List<PropertyData>(propsToAuction);
        StartNextAuction();
    }

    private void StartNextAuction()
    {
        if (propertiesToAuction.Count == 0)
            throw new System.Exception("No properties remain to auction");
        highestBid = null;
        propertiesToAuction[0].EnablePropertyDisplay(propertyDisplay);
        backNextControl.ResetIndex();
        backNextControl.MaxIndex = propertiesToAuction.Count;
        bidMenu.ResetMenu();
        messageLog.LogEventLocal("Starting auction for " +
            propertiesToAuction[0].PropertyName);
    }

    private void AcceptBid(Bid bid)
    {
        highestBid = bid;
        bidMenu.OnBidAccepted(highestBid);
    }

    private void OnPlayersReady()
    {
        bidMenu.LocalPlayer = playerManager.LocalPlayer;
        bidMenu.RemotePlayers = new List<MonopolyPlayer>(playerManager.RemotePlayers);
        bidMenu.Withdrew += OnPlayerWithdrew;
    }

    private void OnPlayerWithdrew(MonopolyPlayer player)
    {
        pView.RPC("RPC_PlayerWithdrew", RpcTarget.OthersBuffered, player.PlayerID);
    }

    private void OnAuctionComplete()
    {
        messageLog.LogEventLocal($"Auction for {CurrentAuctioningProperty.PropertyName} " +
            $"complete. You won the auction with a bid of {highestBid.BidAmount.ToPokeMoneyString()}");
        messageLog.LogEventOthers($"Auction for {CurrentAuctioningProperty.PropertyName} " +
            $"complete. {highestBid.BiddingPlayer.PlayerName} won the auction " +
            $"with a bid of {highestBid.BidAmount.ToPokeMoneyString()}");
        pView.RPC("RPC_AuctionComplete", RpcTarget.AllBuffered);
    }

    private void OnAllAuctionsComplete()
    {
        Close();
    }

    private void OnBidSubmitted(Bid bid)
    {
        pView.RPC("RPC_SubmitBid", RpcTarget.AllBufferedViaServer,
            PhotonNetwork.LocalPlayer, bid.BiddingPlayer.PlayerID, bid.BidAmount);
    }

    private void OnPropertyIndexChanged(int newIndex)
    {
        propertiesToAuction[newIndex - 1].EnablePropertyDisplay(propertyDisplay);
        if (newIndex == 1)
            auctioningWhenText.text = "Currently Auctioning:";
        else if (newIndex == 2)
            auctioningWhenText.text = "Auctioning Next:";
        else
            auctioningWhenText.text = $"Auctioning Later:";
    }

    [PunRPC]
    private void RPC_SubmitBid(Player photonPlayer, int playerID, int bidAmount)
    {
        Debug.Log($"Received bid of {bidAmount} from player {playerID}");
        if (highestBid == null || bidAmount > highestBid.BidAmount)
        {
            MonopolyPlayer biddingPlayer = playerManager.GetPlayerByID(playerID);
            AcceptBid(new Bid(biddingPlayer, bidAmount));
        }
        else
        {
            pView.RPC("RPC_RejectBid", photonPlayer, playerID, bidAmount,
                "An equal or higher bid already exists");
        }
    }

    [PunRPC]
    private void RejectBid(int biddingPlayerID, int bidAmout, string rejectMessage)
    {
        MonopolyPlayer bidder = playerManager.GetPlayerByID(biddingPlayerID);
        bidMenu.OnBidRejected(new Bid(bidder, bidAmout), rejectMessage);
    }

    [PunRPC]
    private void RPC_PlayerWithdrew(int playerID)
    {
        bidMenu.OnRemotePlayerWithdrew(playerManager.GetPlayerByID(playerID));
    }

    [PunRPC]
    private void RPC_AuctionComplete()
    {
        if (highestBid.BiddingPlayer.IsLocalPlayer)
        {
            highestBid.BiddingPlayer.PurchasePropertyFromAuction(
                CurrentAuctioningProperty, highestBid.BidAmount);
        }
        propertiesToAuction.Remove(CurrentAuctioningProperty);
        if (propertiesToAuction.Count == 0) OnAllAuctionsComplete();
        else StartNextAuction();
    }
}

public class Bid
{
    public MonopolyPlayer BiddingPlayer { get; private set; }
    public int BidAmount { get; private set; }

    public Bid(MonopolyPlayer bidder, int bidAmount)
    {
        BiddingPlayer = bidder;
        BidAmount = bidAmount;
    }

    public static bool operator >(Bid a, Bid b) => a.BidAmount > b.BidAmount;
    public static bool operator <(Bid a, Bid b) => a.BidAmount < b.BidAmount;
    public static bool operator >=(Bid a, Bid b) => a.BidAmount >= b.BidAmount;
    public static bool operator <=(Bid a, Bid b) => a.BidAmount <= b.BidAmount;

    public int CompareTo(Bid other)
    {
        if (BidAmount < other.BidAmount) return -1;
        if (BidAmount > other.BidAmount) return 1;
        return 0;
    }
}