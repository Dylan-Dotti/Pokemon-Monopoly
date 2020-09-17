using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoardSpawner))]
public class MonopolyBoard : MonoBehaviour
{
    public event UnityAction<MonopolyBoard> BoardSpawned;
    private IReadOnlyList<BoardSquare> boardSquares;
    private BoardSpawner spawner;

    private void Awake()
    {
        spawner = GetComponent<BoardSpawner>();
        SpawnBoard();
    }

    public void SpawnBoard()
    {
        boardSquares = spawner.SpawnBoard();
        BoardSpawned?.Invoke(this);
    }

    public BoardSquare GetSpawnSquare()
    {
        return boardSquares[0];
    }

    public IReadOnlyList<BoardSquare> GetNextSquares(
        PlayerAvatar avatar, int numSquares)
    {
        List<BoardSquare> nextSquares = new List<BoardSquare>();
        int startIndex = boardSquares.ToList().IndexOf(
            avatar.OccupiedSquare) + 1;
        for (int i = 0; i < numSquares; i++)
        {
            nextSquares.Add(boardSquares[
                (startIndex + i) % boardSquares.Count]);
        }
        return nextSquares;
    }
}
