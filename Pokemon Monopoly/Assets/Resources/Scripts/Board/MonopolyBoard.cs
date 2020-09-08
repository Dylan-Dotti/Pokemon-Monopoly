using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoardSpawner))]
public class MonopolyBoard : MonoBehaviour
{
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
