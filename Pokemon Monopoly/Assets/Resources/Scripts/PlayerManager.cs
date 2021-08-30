using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhotonView))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public event UnityAction PlayersReady;
    public event UnityAction<MonopolyPlayer> ActivePlayerChanged;

    private HashSet<MonopolyPlayer> players;
    private List<MonopolyPlayer> defaultPlayerSequence;
    private Queue<MonopolyPlayer> playerTurnQueue;
    private PhotonView pView;

    public IReadOnlyList<MonopolyPlayer> PlayerTurnSequence => defaultPlayerSequence;
    public MonopolyPlayer ActivePlayer { get; private set; }

    public MonopolyPlayer LocalPlayer => players.SingleOrDefault(p => p.IsLocalPlayer);
    public IReadOnlyList<MonopolyPlayer> RemotePlayers => 
        defaultPlayerSequence.Where(p => !p.IsLocalPlayer).ToList();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            pView = GetComponent<PhotonView>();
            players = new HashSet<MonopolyPlayer>();
            MonopolyPlayer.Spawned += OnPlayerSpawned;
        }
    }

    public MonopolyPlayer GetPlayerByID(int id) =>
        players.Single(p => p.PlayerID == id);

    public IReadOnlyCollection<MonopolyPlayer> GetOpponents(MonopolyPlayer player) =>
        players.Where(p => p.PlayerID != player.PlayerID).ToList();

    // called by GameManager on all clients when turn ends
    // called on both clients when player goes bankrupt
    public void SwitchNextActivePlayerLocal()
    {
        if (ActivePlayer != null) ActivePlayer.OnTurnEnd();
        MonopolyPlayer nextPlayer = playerTurnQueue.Dequeue();
        playerTurnQueue.Enqueue(nextPlayer);
        ActivePlayer = nextPlayer;
        Debug.Log("Active player is now: " + ActivePlayer.PlayerName);
        ActivePlayer.OnTurnStart();
        ActivePlayerChanged?.Invoke(ActivePlayer);
    }

    private IReadOnlyList<int> GenerateRandomPlayerIDSequence() =>
        players.Select(p => p.PlayerID).Randomized();

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        if (players.Add(player))
        {
            player.Despawned += OnPlayerDespawned;
            player.WentBankrupt += OnPlayerWentBankrupt;
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Players count: " + players.Count);
                int id = players.Count == 1 ?
                    1 : players.Select(p => p.PlayerID).Max() + 1;
                player.InitPlayerIdAllClients(id);
                if (players.Count == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    // when all players have spawned, initialize and signal ready
                    pView.RPC("RPC_OnAllPlayersSpawned", RpcTarget.AllBuffered,
                        GenerateRandomPlayerIDSequence().ToArray());
                }
            }
            player.SpawnAvatar();
        }
    }

    private void OnPlayerWentBankrupt(MonopolyPlayer player)
    {
        defaultPlayerSequence.Remove(player);
        var playerList = playerTurnQueue.ToList();
        playerList.Remove(player);
        playerTurnQueue = new Queue<MonopolyPlayer>(playerList);
        ActivePlayer = null;
        if (playerTurnQueue.Count > 0)
        {
            SwitchNextActivePlayerLocal();
        }
    }

    private void OnPlayerDespawned(MonopolyPlayer player)
    {
        players.Remove(player);
    }

    [PunRPC]
    private void RPC_OnAllPlayersSpawned(int[] playerIdSequence)
    {
        playerTurnQueue = new Queue<MonopolyPlayer>();
        foreach (int playerId in playerIdSequence)
        {
            playerTurnQueue.Enqueue(GetPlayerByID(playerId));
        }
        defaultPlayerSequence = playerTurnQueue.ToList();
        PlayersReady?.Invoke();
    }
}
