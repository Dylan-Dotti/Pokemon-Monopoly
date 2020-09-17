﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private DiceRoller roller;

    private MonopolyPlayer friendlyPlayer;
    private PhotonView pView;

    private PlayerAvatar FriendlyAvatar => friendlyPlayer.PlayerToken;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        MonopolyPlayer.Spawned += OnPlayerSpawned;
    }

    public void RollDice()
    {
        roller.RollComplete += OnRollComplete;
        roller.RollDice(friendlyPlayer.PlayerName);
    }

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        if (player.IsLocalPlayer) friendlyPlayer = player;
    }

    private void OnRollComplete(DiceRoller roller)
    {
        roller.RollComplete -= OnRollComplete;
        friendlyPlayer.MoveAvatarSequential(roller.LastRollTotal);
    }
}