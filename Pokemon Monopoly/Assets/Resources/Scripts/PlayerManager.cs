using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhotonView))]
public class PlayerManager : MonoBehaviour
{
    public event UnityAction PlayersReady;
    public event UnityAction<MonopolyPlayer> ActivePlayerChanged;

    private HashSet<MonopolyPlayer> players;
    private List<MonopolyPlayer> defaultPlayerSequence;
    private Queue<MonopolyPlayer> playerTurnQueue;
    private PhotonView pView;

    public IEnumerable<MonopolyPlayer> PlayerTurnSequence => defaultPlayerSequence;
    public MonopolyPlayer ActivePlayer { get; private set; }
    public MonopolyPlayer LocalPlayer => 
        players.Where(p => p.IsLocalPlayer).FirstOrDefault();
    public IEnumerable<MonopolyPlayer> RemotePlayers => 
        defaultPlayerSequence.Where(p => !p.IsLocalPlayer);

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        players = new HashSet<MonopolyPlayer>();
        MonopolyPlayer.Spawned += OnPlayerSpawned;
    }

    public MonopolyPlayer GetPlayerByName(string playerName)
    {
        return players.Where(p => p.PlayerName == playerName)
            .FirstOrDefault();
    }

    public IReadOnlyCollection<MonopolyPlayer> GetOpponents(string playerName)
    {
        return players.Where(p => p.PlayerName != playerName).ToList();
    }

    public void SwitchNextActivePlayer()
    {
        MonopolyPlayer nextPlayer = playerTurnQueue.Dequeue();
        ActivePlayer = nextPlayer;
        playerTurnQueue.Enqueue(nextPlayer);
        Debug.Log("Active player is now: " + ActivePlayer.PlayerName);
        ActivePlayerChanged?.Invoke(ActivePlayer);
    }

    private void AssignPlayerTurns()
    {
        List<string> playerNames = players.ToList()
            .Select(p => p.PlayerName).ToList();
        List<string> nameSequence = new List<string>();
        while (nameSequence.Count < players.Count)
        {
            int randIndex = Random.Range(0, playerNames.Count);
            if (!nameSequence.Contains(playerNames[randIndex]))
            {
                nameSequence.Add(playerNames[randIndex]);
            }
        }
        if (players.Count == 1)
        {
            AssignTurnsFromSequence(nameSequence);
        }
        else
        {
            pView.RPC("RPC_AssignTurns", RpcTarget.AllBuffered,
                nameSequence.ToArray());
        }
    }

    private void AssignTurnsFromSequence(IEnumerable<string> nameSequence)
    {
        Debug.Log("Player sequence:");
        playerTurnQueue = new Queue<MonopolyPlayer>();
        nameSequence.ToList().ForEach(
            n => playerTurnQueue.Enqueue(GetPlayerByName(n)));
        defaultPlayerSequence = new List<MonopolyPlayer>(playerTurnQueue);
        foreach (MonopolyPlayer player in playerTurnQueue)
        {
            Debug.Log(player.PlayerName);
        }
        PlayersReady?.Invoke();
    }

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        if (players.Add(player))
        {
            player.Despawned += OnPlayerDespawned;
            if (players.Count == PhotonNetwork.CurrentRoom.PlayerCount
                && PhotonNetwork.IsMasterClient)
            {
                AssignPlayerTurns();
            }
        }
    }

    private void OnPlayerDespawned(MonopolyPlayer player)
    {
        players.Remove(player);
    }

    [PunRPC]
    private void RPC_AssignTurns(string[] playerNameSequence)
    {
        AssignTurnsFromSequence(playerNameSequence);
    }
}
