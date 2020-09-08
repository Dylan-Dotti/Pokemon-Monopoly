using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MonopolyBoard board;

    private MonopolyPlayer friendlyPlayer;

    private void Awake()
    {
        MonopolyPlayer.Spawned += OnPlayerSpawned;
    }

    public void MovePlayerTo(BoardSquare square)
    {
        friendlyPlayer.Avatar.MoveToSquare(square);
    }

    public void MoveAvatarForward(int numSquares)
    {
        IReadOnlyList<BoardSquare> nextSquares =
            board.GetNextSquares(
                friendlyPlayer.Avatar, numSquares);
        foreach (BoardSquare square in nextSquares)
        {
            MovePlayerTo(square);
        }
    }

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        friendlyPlayer = player;
        StartCoroutine(TestMoves());
    }

    private IEnumerator TestMoves()
    {
        friendlyPlayer.Avatar.SpawnAtSquare(board.GetSpawnSquare());
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Moving avatar forward");
            MoveAvatarForward(1);
        }
    }
}
