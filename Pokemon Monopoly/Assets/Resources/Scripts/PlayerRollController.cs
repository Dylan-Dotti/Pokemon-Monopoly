﻿using UnityEngine;

public class PlayerRollController : MonoBehaviour
{
    [SerializeField] private DiceRoller roller;

    private MonopolyPlayer friendlyPlayer;

    private void Awake()
    {
        MonopolyPlayer.Spawned += OnPlayerSpawned; 
    }

    public void RollDice()
    {
        roller.RollComplete += OnRollComplete;
        roller.RollDice(friendlyPlayer == null ? "Player" : friendlyPlayer.PlayerName);
    }

    public void RollDice(int result1, int result2)
    {
        roller.RollComplete += OnRollComplete;
        roller.RollDice(friendlyPlayer == null ? 
            "Player" : friendlyPlayer.PlayerName,
            result1, result2);
    }

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        if (player.IsLocalPlayer) friendlyPlayer = player;
    }

    private void OnRollComplete(DiceRoll roll)
    {
        roller.RollComplete -= OnRollComplete;
        if (friendlyPlayer != null)
        {
            if (friendlyPlayer.InJail)
            {
                if (!roll.IsDoubleRoll) return;
                friendlyPlayer.LeaveJailAllClients();
            }
            friendlyPlayer.MoveAvatarSequentialAllClients(roll.RollTotal);
        }
    }
}
