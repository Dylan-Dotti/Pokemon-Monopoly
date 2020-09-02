using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public event UnityAction LocalPlayerJoined;
    public event UnityAction LocalPlayerLeft;

    public event UnityAction RemotePlayerJoined;
    public event UnityAction RemotePlayerLeft;

    public event UnityAction ReachedMinPlayers;
    public event UnityAction BelowMinPlayers;
    public event UnityAction ReachedMaxPlayers;

    //player info
    public int PlayersInRoom { get; private set; }
    public int PlayersInGame { get; private set; }

    public int MinPlayers { get => minPlayers; }
    public int MaxPlayers { get; private set; }

    private Player[] players;
    [SerializeField] private int minPlayers = 1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        MaxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
        players = PhotonNetwork.PlayerList;
        PlayersInRoom = players.Length;
        Debug.Log($"{PlayersInRoom}/{MaxPlayers}");
        LocalPlayerJoined?.Invoke();
        CheckPlayerCountEvents(true, false, true);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        LocalPlayerLeft?.Invoke();
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            PhotonNetwork.LoadLevel(0);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("Remote player joined room");
        players = PhotonNetwork.PlayerList;
        PlayersInRoom = players.Length;
        Debug.Log($"{PlayersInRoom}/{MaxPlayers}");
        RemotePlayerJoined?.Invoke();
        CheckPlayerCountEvents(true, false, true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        players = PhotonNetwork.PlayerList;
        PlayersInRoom = players.Length;
        Debug.Log($"{PlayersInRoom}/{MaxPlayers}");
        RemotePlayerLeft?.Invoke();
        CheckPlayerCountEvents(false, true, false);
    }

    private void CheckPlayerCountEvents(
        bool checkReachedMin, bool checkBelowMin, bool checkMax)
    {
        if (checkReachedMin && PlayersInRoom == minPlayers)
        {
            ReachedMinPlayers?.Invoke();
        }
        if (checkBelowMin && PlayersInRoom < minPlayers)
        {
            BelowMinPlayers?.Invoke();
        }
        if (checkMax && PlayersInRoom == MaxPlayers)
        {
            ReachedMaxPlayers?.Invoke();
        }
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == MultiplayerSettings.Instance.MultiplayerScene)
        {
            photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        Debug.Log("Loaded game scene");
        PlayersInGame++;
        // spawn player objects if everyone is in game
        if (PlayersInGame == PhotonNetwork.PlayerList.Length)
        {
            photonView.RPC("RPC_CreatePlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        Debug.Log("Creating player");
        PhotonNetwork.Instantiate(Path.Combine(
            "Prefabs", "Photon Network Player"), 
            transform.position, Quaternion.identity, 0);
    }
}
