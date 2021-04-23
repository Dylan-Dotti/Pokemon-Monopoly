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
    private DecksManager deckManager;
    private BoardSpawner spawner;

    private void Awake()
    {
        spawner = GetComponent<BoardSpawner>();
        deckManager = GetComponentInChildren<DecksManager>(true);
        SpawnBoard();
    }

    public void SpawnBoard()
    {
        boardSquares = spawner.SpawnBoard();
        boardSquares.ForEach(s => s.ParentBoard = this);
        deckManager.SpawnDecks();
        BoardSpawned?.Invoke(this);
    }

    public BoardSquare GetGoSquare()
    {
        return GetSquareAt(0);
    }

    public BoardSquare GetJailSquare()
    {
        return GetSquareAt(10);
    }

    public BoardSquare GetSquareAt(int index)
    {
        return boardSquares[index];
    }

    public int IndexOf(BoardSquare square)
    {
        return boardSquares.IndexOf(square);
    }

    public IReadOnlyList<BoardSquare> GetPathTo(
            BoardSquare startSquare, BoardSquare targetSquare, bool backwards = false)
    {
        List<BoardSquare> pathSquares = new List<BoardSquare>();
        for (int i = 0, currentIndex = IndexOf(startSquare); i < boardSquares.Count; i++)
        {
            currentIndex = GetNextSquareIndex(currentIndex, backwards);
            BoardSquare nextSquare = boardSquares[currentIndex];
            pathSquares.Add(nextSquare);
            if (nextSquare == targetSquare) break;
        }
        return pathSquares;
    }

    public IReadOnlyList<BoardSquare> GetNextSquares(
        BoardSquare startSquare, int numSquares, bool reversed = false)
    {
        List<BoardSquare> nextSquares = new List<BoardSquare>();
        for (int i = 0, currentIndex = IndexOf(startSquare); i < numSquares; i++)
        {
            currentIndex = GetNextSquareIndex(currentIndex, reversed);
            nextSquares.Add(boardSquares[currentIndex]);
        }
        return nextSquares;
    }

    private int GetNextSquareIndex(int startIndex, bool reversed = false)
    {
        int index = startIndex + (reversed ? -1 : 1);
        index = index >= 0 ? index : boardSquares.Count - 1;
        return index % boardSquares.Count;
    }

    public Card DrawProfessorCard(MonopolyPlayer drawingPlayer)
    {
        return deckManager.DrawProfessorCard(drawingPlayer);
    }

    public Card DrawTrainerBattleCard(MonopolyPlayer drawingPlayer)
    {
        return deckManager.DrawTrainerBattleCard(drawingPlayer);
    }
}
