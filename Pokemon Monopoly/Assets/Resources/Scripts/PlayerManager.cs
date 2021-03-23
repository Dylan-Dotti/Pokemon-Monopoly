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

    private PlayerUIManager localPlayerUI;

    private HashSet<MonopolyPlayer> players;
    private List<MonopolyPlayer> defaultPlayerSequence;
    private Queue<MonopolyPlayer> playerTurnQueue;
    private PhotonView pView;

    public IEnumerable<MonopolyPlayer> PlayerTurnSequence => defaultPlayerSequence;
    public MonopolyPlayer ActivePlayer { get; private set; }

    public MonopolyPlayer LocalPlayer => 
        players.Where(p => p.IsLocalPlayer).FirstOrDefault();
    public IReadOnlyList<MonopolyPlayer> RemotePlayers => 
        defaultPlayerSequence.Where(p => !p.IsLocalPlayer).ToList();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            localPlayerUI = PlayerUIManager.Instance;
            pView = GetComponent<PhotonView>();
            players = new HashSet<MonopolyPlayer>();
            MonopolyPlayer.Spawned += OnPlayerSpawned;
        }
    }

    public MonopolyPlayer GetPlayerByID(int id)
    {
        return players.Single(p => p.PlayerID == id);
    }

    public IReadOnlyCollection<MonopolyPlayer> GetOpponents(MonopolyPlayer player)
    {
        return players.Where(p => p.PlayerID != player.PlayerID).ToList();
    }

    public void SwitchNextActivePlayer()
    {
        MonopolyPlayer nextPlayer = playerTurnQueue.Dequeue();
        playerTurnQueue.Enqueue(nextPlayer);
        ActivePlayer = nextPlayer;
        if (ActivePlayer.IsLocalPlayer)
        {
            localPlayerUI.RollButtonInteractable = true;
        }
        Debug.Log("Active player is now: " + ActivePlayer.PlayerName);
        ActivePlayerChanged?.Invoke(ActivePlayer);
    }

    private IReadOnlyList<int> GenerateRandomPlayerIDSequence()
    {
        return players.Select(p => p.PlayerID).Randomized();
    }

    private void AssignTurnsFromSequence(IEnumerable<int> idSequence)
    {
        playerTurnQueue = new Queue<MonopolyPlayer>();
        idSequence.ToList().ForEach(
            n => playerTurnQueue.Enqueue(GetPlayerByID(n)));
        defaultPlayerSequence = new List<MonopolyPlayer>(playerTurnQueue);
    }

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        if (players.Add(player))
        {
            player.Manager = this;
            player.Despawned += OnPlayerDespawned;
            if (PhotonNetwork.IsMasterClient)
            {
                player.PlayerID = players.Count == 1 ?
                    1 : players.Select(p => p.PlayerID).Max() + 1;
                if (players.Count == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    // when all players have spawned, initialize and signal ready
                    pView.RPC("RPC_InitPlayers", RpcTarget.AllBuffered,
                        GenerateRandomPlayerIDSequence().ToArray());
                }
            }
            player.SpawnAvatar();
        }
    }

    private void OnPlayerDespawned(MonopolyPlayer player)
    {
        players.Remove(player);
    }

    [PunRPC]
    private void RPC_InitPlayers(int[] playerIdSequence)
    {
        AssignTurnsFromSequence(playerIdSequence);
        PlayersReady?.Invoke();
    }
}
