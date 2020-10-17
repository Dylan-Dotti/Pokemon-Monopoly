using Photon.Pun;
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

    private void OnRollComplete(DiceRoller roller)
    {
        roller.RollComplete -= OnRollComplete;
        if (friendlyPlayer != null)
        {
            friendlyPlayer.MoveAvatarAllClients(
                roller.LastRollTotal, MoveDirection.Forward);
        }
    }
}
