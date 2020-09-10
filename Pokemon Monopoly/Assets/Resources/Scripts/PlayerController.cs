using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private MonopolyBoard board;
    [SerializeField] private DiceRoller roller;

    private MonopolyPlayer friendlyPlayer;
    private PhotonView pView;

    private PlayerAvatar FriendlyAvatar => friendlyPlayer.Avatar;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        MonopolyPlayer.Spawned += OnPlayerSpawned;
        roller.RollComplete += OnRollComplete;
    }

    public void RollDice()
    {
        roller.RollDice(friendlyPlayer.PlayerName);
    }

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        friendlyPlayer = player;
        friendlyPlayer.Avatar.SpawnAtSquare(
            board.GetSpawnSquare());
    }

    private void OnRollComplete(DiceRoller roller)
    {
        IReadOnlyList<BoardSquare> nextSquares = 
            board.GetNextSquares(
                friendlyPlayer.Avatar, roller.LastRollTotal);
        friendlyPlayer.MoveAvatarSequential(nextSquares);
    }
}
