using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BiddingMenu : MonoBehaviour
{
    public event UnityAction<Bid> BidSubmitted;
    public event UnityAction<MonopolyPlayer> Withdrew;
    public event UnityAction AuctionComplete;

    [Header("Highest Bid Display")]
    [SerializeField] private Text bidAmountText;
    [SerializeField] private Text bidderNameText;
    [SerializeField] private RectTransform avatarImageHolder;

    [Header("Multiplayer Events")]
    [SerializeField] private MultiplayerMessageLog messageLog;

    [Header("Remote Bidders")]
    [SerializeField] private BiddingMenuRemoteBidder[] remoteBidders;

    [Header("Bid Input")]
    [SerializeField] private InputField bidInput;
    [SerializeField] private Button bidButton;
    [SerializeField] private Button withdrawButton;

    [Header("Bid Result Messages")]
    [SerializeField] private Text bidResultText;
    [SerializeField] private Color errorColor;
    private Color defaultColor;

    // players not added back on reset
    private List<MonopolyPlayer> withdrawnRemotePlayers;
    private List<MonopolyPlayer> remotePlayers;

    public MonopolyPlayer LocalPlayer { get; set; }
    public IReadOnlyList<MonopolyPlayer> RemotePlayers
    {
        set
        {
            remotePlayers.Clear();
            for (int i = 0; i < value.Count; i++)
            {
                MonopolyPlayer player = value[i];
                remotePlayers.Add(player);
                remoteBidders[i].gameObject.SetActive(true);
                remoteBidders[i].LinkedPlayer = player;
            }
        }
    }

    private void Awake()
    {
        withdrawnRemotePlayers = new List<MonopolyPlayer>();
        defaultColor = bidResultText.color;
        remotePlayers = new List<MonopolyPlayer>();
        bidButton.onClick.AddListener(SubmitBid);
        withdrawButton.onClick.AddListener(Withdraw);
    }

    public void Withdraw()
    {
        SetControlsEnabled(false);
        PublishResultMessage("You have withdrawn from this auction");
        messageLog.LogEventLocal("You have withdrawn from this auction");
        Withdrew?.Invoke(LocalPlayer);
    }

    public void OnBidAccepted(Bid bid)
    {
        if (bid.BiddingPlayer == LocalPlayer)
        {
            SetControlsEnabled(true);
            bidInput.text = "";
            bidResultText.color = defaultColor;
            PublishResultMessage(
                $"Bid for {bid.BidAmount.ToPokeMoneyString()} accepted");
            messageLog.LogEventLocal("You bid " + bid.BidAmount.ToPokeMoneyString());
            messageLog.LogEventOthers(bid.BiddingPlayer.PlayerName +
                " bid " + bid.BidAmount.ToPokeMoneyString());
        }
        withdrawButton.interactable = bid.BiddingPlayer != LocalPlayer;
        bidAmountText.text = bid.BidAmount.ToPokeMoneyString();
        bidderNameText.text = bid.BiddingPlayer.PlayerName;
        avatarImageHolder.GetChildren().ForEach(c => Destroy(c.gameObject));
        bid.BiddingPlayer.GetNewAvatarImage(avatarImageHolder, Vector3.one * 0.35f);
        if (withdrawnRemotePlayers.Count == remotePlayers.Count) AuctionComplete?.Invoke();
    }

    public void OnBidRejected(Bid bid, string rejectReason)
    {
        SetControlsEnabled(true);
        bidInput.text = "";
        PublishBidError($"Bid for {bid.BidAmount.ToPokeMoneyString()} rejected " +
            System.Environment.NewLine + rejectReason);
    }

    public void OnRemotePlayerWithdrew(MonopolyPlayer player)
    {
        if (remotePlayers.Contains(player))
        {
            messageLog.LogEventLocal(player.PlayerName + " has withdrawn from this auction");
            remoteBidders[remotePlayers.IndexOf(player)].gameObject.SetActive(false);
            withdrawnRemotePlayers.Add(player);
            if (withdrawnRemotePlayers.Count == remotePlayers.Count) AuctionComplete?.Invoke();
        }
    }

    public void ResetMenu()
    {
        withdrawnRemotePlayers.Clear();
        SetControlsEnabled(true);
        withdrawButton.interactable = false;
        bidResultText.text = "";
        bidAmountText.text = 0.ToPokeMoneyString();
        bidderNameText.text = "None";
        avatarImageHolder.GetChildren().ForEach(c => Destroy(c.gameObject));
        for (int i = 0; i < remotePlayers.Count; i++)
        {
            remoteBidders[i].gameObject.SetActive(true);
        }
    }

    private void SubmitBid()
    {
        if (bidInput.text.Length == 0)
        {
            PublishBidError("Can't submit an empty bid value");
            return;
        }
        int bidAmount = int.Parse(bidInput.text);
        if (bidAmount > LocalPlayer.Money)
        {
            PublishBidError("Can't bid higher than your available funds");
            return;
        }
        SetControlsEnabled(false);
        BidSubmitted?.Invoke(new Bid(LocalPlayer, bidAmount));
    }

    private void SetControlsEnabled(bool enabled)
    {
        bidInput.interactable = enabled;
        bidButton.interactable = enabled;
        withdrawButton.interactable = enabled;
    }

    private void PublishResultMessage(string message)
    {
        bidResultText.color = defaultColor;
        bidResultText.text = message;
    }

    private void PublishBidError(string errorMessage)
    {
        bidResultText.color = errorColor;
        bidResultText.text = errorMessage;
    }
}
