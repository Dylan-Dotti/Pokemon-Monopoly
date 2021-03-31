using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarController : MonoBehaviour
{
    [SerializeField] private PlayerAvatar avatarPrefab;

    private MonopolyBoard board;

    public PlayerAvatar Avatar { get; private set; }

    private void Awake()
    {
        board = GameObject.FindWithTag("Board")
            .GetComponent<MonopolyBoard>();
    }

    public void SpawnAvatar(MonopolyPlayer owner)
    {
        Debug.Log("Spawning avatar");
        Avatar = Instantiate(avatarPrefab);
        Avatar.Owner = owner;
        Avatar.SpawnAtSquare(board.GetGoSquare());
    }

    public void DespawnAvatar()
    {
        Debug.Log("Despawning avatar");
        Destroy(Avatar);
    }

    public Coroutine MoveAvatarSequentialLocal(int numSquares, bool reversed = false)
    {
        IReadOnlyList<BoardSquare> squareSequence = board.GetNextSquares(
            Avatar.OccupiedSquare, numSquares, reversed);
        return Avatar.MoveSequential(squareSequence, 0.5f);
    }

    public void MoveToJailSquare()
    {
        Avatar.MoveToSquare(board.GetJailSquare(), true);
    }

    public Coroutine LerpToJailSquare(bool hideDuringMove = false)
    {
        return Avatar.LerpToSquare(
            board.GetJailSquare(), hideDuringMove: hideDuringMove);
    }
}
