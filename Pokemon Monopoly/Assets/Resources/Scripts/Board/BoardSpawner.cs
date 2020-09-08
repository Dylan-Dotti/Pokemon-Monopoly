﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardSpawner : MonoBehaviour
{
    [Header("Square Factory")]
    [SerializeField] private BoardSquareFactory factory;

    [Header("Square Spacing")]
    [SerializeField] private float squareSpacing = 0.05f;

    private float BoardSize => (2 * factory.CornerSquareSize) +
        (9 * factory.StreetSquareWidth) + (10 * squareSpacing);
    private float CenterToRowMiddleDist => BoardSize / 2 - factory.CornerSquareSize / 2;

    public IReadOnlyList<BoardSquare> SpawnBoard()
    {
        Debug.Log("Spawning board");
        Debug.Log($"Board size: ({BoardSize} * {BoardSize})");

        List<BoardSquare> squares = new List<BoardSquare>();
        for (int i = 0; i < 4; i++)
        {
            squares.AddRange(SpawnRow(i));
        }
        return squares;
    }

    public IReadOnlyList<BoardSquare> SpawnRow(int rowIndex)
    {
        int startIndex = 10 * rowIndex;
        List<BoardSquare> rowSquares = new List<BoardSquare>();
        List<Vector2> rowCoords = GetRowCoords(rowIndex);
        Vector2 cornerCoords = rowCoords[0];
        List<Vector2> propertyCoords = rowCoords.GetRange(1, rowCoords.Count - 1);
        rowSquares.Add(factory.SpawnSquare(
            startIndex, cornerCoords, GetSpawnRotation(rowIndex), transform));
        for (int i = 0; i < propertyCoords.Count; i++)
        {
            rowSquares.Add(factory.SpawnSquare(
                startIndex + i + 1, propertyCoords[i], GetSpawnRotation(rowIndex), transform));
        }
        return rowSquares;
    }

    private List<Vector2> GetRowCoords(int rowIndex)
    {
        Vector2 cornerCoords;
        IEnumerable<Vector2> propertyCoords;
        float cornerCenterToPropertyCenterDist = (factory.CornerSquareSize / 2) + 
            squareSpacing + (factory.StreetSquareWidth / 2);
        float distBetweenProperties = squareSpacing + factory.StreetSquareWidth;
        IEnumerable<float> propertyDistScalars = Enumerable.Range(0, 9).Select(
            i => cornerCenterToPropertyCenterDist + i * distBetweenProperties);
        switch (rowIndex)
        {
            case 0:
                cornerCoords = new Vector2(CenterToRowMiddleDist, -CenterToRowMiddleDist);
                propertyCoords = propertyDistScalars.Select(scalar => new Vector2(
                    cornerCoords.x - scalar, cornerCoords.y));
                break;
            case 1:
                cornerCoords = new Vector2(-CenterToRowMiddleDist, -CenterToRowMiddleDist);
                propertyCoords = propertyDistScalars.Select(scalar => new Vector2(
                    cornerCoords.x, cornerCoords.y + scalar));
                break;
            case 2:
                cornerCoords = new Vector2(-CenterToRowMiddleDist, CenterToRowMiddleDist);
                propertyCoords = propertyDistScalars.Select(scalar => new Vector2(
                    cornerCoords.x + scalar, cornerCoords.y));
                break;
            case 3:
                cornerCoords = new Vector2(CenterToRowMiddleDist, CenterToRowMiddleDist);
                propertyCoords = propertyDistScalars.Select(scalar => new Vector2(
                    cornerCoords.x, cornerCoords.y - scalar));
                break;
            default:
                throw new System.ArgumentException($"Unexpected index: {rowIndex}");
        }
        List<Vector2> rowCoords = new List<Vector2>();
        rowCoords.Add(cornerCoords);
        rowCoords.AddRange(propertyCoords);
        return rowCoords;
    }

    private Quaternion GetSpawnRotation(int rowIndex)
    {
        switch (rowIndex)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return Quaternion.Euler(0, 0, -90f * rowIndex);
            default:
                throw new System.ArgumentException($"Unexpected index: {rowIndex}");
        }
    }
}
