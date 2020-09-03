using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoardSpawner))]
public class MonopolyBoard : MonoBehaviour
{
    private IReadOnlyList<BoardSquare> boardSquares;
    private BoardSpawner spawner;

    private void Awake()
    {
        spawner = GetComponent<BoardSpawner>();
    }

    private void Start()
    {
        SpawnBoard();
    }

    public void SpawnBoard()
    {
        boardSquares = spawner.SpawnBoard();
    }
}
