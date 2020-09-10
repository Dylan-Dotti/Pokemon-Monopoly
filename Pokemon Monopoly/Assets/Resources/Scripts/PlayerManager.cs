using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private HashSet<MonopolyPlayer> players;

    private void Awake()
    {
        players = new HashSet<MonopolyPlayer>();
        MonopolyPlayer.Spawned += OnPlayerSpawned;
    }

    public MonopolyPlayer GetPlayerByName(string playerName)
    {
        return players.Where(p => p.PlayerName == playerName)
            .FirstOrDefault();
    }

    public IReadOnlyCollection<MonopolyPlayer> GetOpponents(
        string playerName)
    {
        return players.Where(p => p.PlayerName != playerName).ToList();
    }

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        if (players.Add(player))
        {
            player.Despawned += OnPlayerDespawned;
        }
    }

    private void OnPlayerDespawned(MonopolyPlayer player)
    {
        players.Remove(player);
    }
}
