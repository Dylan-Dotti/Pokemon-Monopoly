using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAvatarController : MonoBehaviour
{
    public event UnityAction CompletedAllActions;

    [SerializeField] private PlayerAvatar avatarPrefab;

    private ActionQueue actionQueue;
    private MonopolyBoard board;

    public PlayerAvatar Avatar { get; private set; }

    private void Awake()
    {
        board = GameObject.FindWithTag("Board")
            .GetComponent<MonopolyBoard>();
        actionQueue = new ActionQueue(this);
        actionQueue.CompletedAllActions += () => CompletedAllActions?.Invoke();
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
        Avatar.Despawn();
    }

    public void QueueSequentialMove(int numSquares, bool reversed = false)
    {
        IReadOnlyList<BoardSquare> squareSequence = board.GetNextSquares(
            Avatar.OccupiedSquare, numSquares, reversed);
        QueueSequentialMove(squareSequence);
    }

    public void QueueSequentialMove(IReadOnlyList<BoardSquare> squareSequence)
    {
        actionQueue.QueueCoroutineAction(
            () => StartCoroutine(MoveSequentialCR(squareSequence, 0.33f)));
    }

    public void QueueMoveToJailSquare()
    {
        actionQueue.QueueSynchronousAction(() => 
        {
            Avatar.MoveToSquare(board.GetJailSquare(), true);
        });
    }

    public void QueueLerpToJailSquare(bool hideDuringMove = false)
    {
        actionQueue.QueueCoroutineAction(
            () => StartCoroutine(MoveSequentialCR(
            new List<BoardSquare> { board.GetJailSquare() },
            0.5f, hideDuringMove: hideDuringMove)));
    }

    private IEnumerator MoveSequentialCR(
        IReadOnlyList<BoardSquare> squares, float interval,
        bool hideDuringMove = false)
    {
        for (int i = 0; i < squares.Count - 1; i++)
        {
            yield return new WaitForSeconds(interval);
            yield return Avatar.LerpToSquare(squares[i], false, false, hideDuringMove);
            squares[i].ApplyEffects(Avatar.Owner, false);
        }
        yield return new WaitForSeconds(interval);
        yield return Avatar.LerpToSquare(squares[squares.Count - 1], true, false, hideDuringMove);
        yield return new WaitForSeconds(interval);
        squares[squares.Count - 1].ApplyEffects(Avatar.Owner, true);
    }
}
