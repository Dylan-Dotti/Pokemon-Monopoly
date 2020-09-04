using System.Collections.Generic;
using UnityEngine;

public class BoardSquareFactory : MonoBehaviour
{
    [Header("Square Primitives")]
    [SerializeField] private GameObject cornerSquare;
    [SerializeField] private GameObject streetSquare;

    [Header("Corners")]
    [SerializeField] private GoSquare goSquare;

    [Header("Cards")]
    [SerializeField] private CardSquare trainerBattleSquare;
    [SerializeField] private CardSquare professorOakSquare;

    [Header("Gyms")]
    [SerializeField] private BoardSquare geodudeSquare;

    public float CornerSquareSize => cornerSquare.transform.lossyScale.x;
    public float StreetSquareWidth => streetSquare.transform.lossyScale.x;
    public float StreetSquareHeight => streetSquare.transform.lossyScale.y;

    public BoardSquare SpawnSquare(
        int index, Vector3 spawnPos, Quaternion rotation, Transform parent)
    {
        switch (index)
        {
            case 0:
            case 10:
            case 20:
            case 30:
                return Instantiate(goSquare, spawnPos, rotation, parent);
            case 2:
            case 17:
            case 33:
                return Instantiate(trainerBattleSquare, spawnPos, rotation, parent);
            case 7:
            case 22:
            case 36:
                return Instantiate(professorOakSquare, spawnPos, rotation, parent);
            default:
                return Instantiate(trainerBattleSquare, spawnPos, rotation, parent);
        }
    }
}
