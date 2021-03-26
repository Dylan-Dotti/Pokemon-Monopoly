using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PhotonView))]
public class PhotonRoom : MonoBehaviourPunCallbacks
{
    public event UnityAction LocalPlayerJoined;
    public event UnityAction LocalPlayerLeft;

    public event UnityAction RemotePlayerJoined;
    public event UnityAction RemotePlayerLeft;

    public event UnityAction ReachedMinPlayers;
    public event UnityAction BelowMinPlayers;
    public event UnityAction BelowMaxPlayers;
    public event UnityAction ReachedMaxPlayers;

    // sends previous and new values
    public event UnityAction<int, int> NumPlayersChanged;

    [SerializeField] private int minPlayers = 1;

    private Player[] players;

    //player info
    public IReadOnlyList<Player> PlayersInRoom => players.ToList();
    public List<Player> PlayersInGame { get; private set; }

    public int MinPlayers => minPlayers;
    public int MaxPlayers { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PlayersInGame = new List<Player>();
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
        Debug.Log($"{PlayersInRoom.Count}/{MaxPlayers}");
        LocalPlayerJoined?.Invoke();
        CheckPlayerCountEvents(true, false, true);
        NumPlayersChanged?.Invoke(PlayersInRoom.Count - 1, PlayersInRoom.Count);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        LocalPlayerLeft?.Invoke();
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            PhotonNetwork.LoadLevel(0);
        }
        NumPlayersChanged?.Invoke(PlayersInRoom.Count + 1, PlayersInRoom.Count);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("Remote player joined room");
        players = PhotonNetwork.PlayerList;
        Debug.Log($"{PlayersInRoom.Count}/{MaxPlayers}");
        RemotePlayerJoined?.Invoke();
        CheckPlayerCountEvents(true, false, true);
        NumPlayersChanged?.Invoke(PlayersInRoom.Count - 1, PlayersInRoom.Count);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        players = PhotonNetwork.PlayerList;
        Debug.Log($"{PlayersInRoom}/{MaxPlayers}");
        RemotePlayerLeft?.Invoke();
        CheckPlayerCountEvents(false, true, false);
        NumPlayersChanged?.Invoke(PlayersInRoom.Count + 1, PlayersInRoom.Count);
    }

    private void CheckPlayerCountEvents(
        bool checkReachedMin, bool checkBelowMin, bool checkMax)
    {
        if (checkReachedMin && PlayersInRoom.Count == minPlayers)
        {
            ReachedMinPlayers?.Invoke();
        }
        if (checkBelowMin && PlayersInRoom.Count < minPlayers)
        {
            BelowMinPlayers?.Invoke();
        }
        if (checkMax && PlayersInRoom.Count == MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            ReachedMaxPlayers?.Invoke();
        }
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == MultiplayerSettings.Instance.MultiplayerScene)
        {
            int playerId = PhotonNetwork.CurrentRoom.Players
                .Single(kv => kv.Value == PhotonNetwork.LocalPlayer).Key;
            photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient, playerId);
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene(int playerId)
    {
        Debug.Log("Loaded game scene");
        PlayersInGame.Add(PhotonNetwork.CurrentRoom.Players[playerId]);
        // spawn player objects if everyone is in game
        if (PlayersInGame.Count == PhotonNetwork.PlayerList.Length)
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
