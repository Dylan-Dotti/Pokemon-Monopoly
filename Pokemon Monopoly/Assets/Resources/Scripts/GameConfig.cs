using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class GameConfig : MonoBehaviourPunCallbacks
{
    [SerializeField] private int playerStartingMoney = 1500;
    [SerializeField] private int maxNumMarts = 32;
    [SerializeField] private int maxNumCenters = 12;
    [SerializeField] private bool auctionPropertyOnNoBuy = true;

    private PhotonView pView;

    public event UnityAction UpdatedValues;

    public static GameConfig Instance { get; private set; }

    public int PlayerStartingMoney
    {
        get => playerStartingMoney;
        set => playerStartingMoney = value;
    }

    public int MaxNumMarts
    {
        get => maxNumMarts;
        set => maxNumMarts = value;
    }

    public int MaxNumCenters
    {
        get => maxNumCenters;
        set => maxNumCenters = value;
    }

    public bool AuctionPropertyOnNoBuy
    {
        get => auctionPropertyOnNoBuy;
        set => auctionPropertyOnNoBuy = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            pView = GetComponent<PhotonView>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateSettings()
    {

    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (!PhotonNetwork.IsMasterClient)
        {
            pView.RPC("RPC_RequestUpdate", RpcTarget.MasterClient,
                PhotonNetwork.LocalPlayer);
        }
    }

    [PunRPC]
    private void RPC_RequestUpdate(Photon.Realtime.Player requestingPlayer)
    {
        Debug.Log(requestingPlayer.NickName + " requested config update");
        pView.RPC("RPC_UpdateConfig", requestingPlayer,
            PlayerStartingMoney, MaxNumMarts, MaxNumCenters, AuctionPropertyOnNoBuy);
    }

    [PunRPC]
    private void RPC_UpdateConfig(int startingMoney,
        int maxNumMarts, int maxNumCenters, bool auctionUnbought)
    {
        Debug.Log("Updating config");
        PlayerStartingMoney = startingMoney;
        MaxNumMarts = maxNumMarts;
        MaxNumCenters = maxNumCenters;
        AuctionPropertyOnNoBuy = auctionUnbought;
        UpdatedValues?.Invoke();
    }
}
