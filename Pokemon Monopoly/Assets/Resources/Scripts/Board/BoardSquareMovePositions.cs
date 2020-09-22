using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardSquareMovePositions : MonoBehaviour
{
    private Vector3[][] playerMovePositions;

    private void Awake()
    {
        Transform positionsHolder = transform.Find("Player Positions Holder");
        playerMovePositions = new Vector3[positionsHolder.childCount][];
        for (int i = 0; i < positionsHolder.childCount; i++)
        {
            Transform positionsParent = positionsHolder.GetChild(i);
            playerMovePositions[i] = new Vector3[positionsParent.childCount];
            for (int j = 0; j < positionsParent.childCount; j++)
            {
                playerMovePositions[i][j] = positionsParent.GetChild(j).position;
            }
        }
    }

    public IReadOnlyList<Vector3> GetMovePositions(
        IReadOnlyList<MonoBehaviour> players)
    {
        return players.Select(p => GetMovePosition(players, p)).ToList();
    }

    public Vector3 GetMovePosition(IReadOnlyList<MonoBehaviour> currentPlayers,
        MonoBehaviour movingPlayer)
    {
        if (currentPlayers.Contains(movingPlayer))
        {
            return GetMovePosition(currentPlayers.Count,
                currentPlayers.IndexOf(movingPlayer));
        }
        return GetMovePosition(currentPlayers.Count + 1, currentPlayers.Count);
    }

    private Vector3 GetMovePosition(int numPlayers, int playerIndex)
    {
        if (numPlayers > playerMovePositions.Length)
        {
            throw new System.IndexOutOfRangeException(
                $"More than {playerMovePositions.Length} players are not supported");
        }
        Vector3[] movePositions = playerMovePositions[numPlayers - 1];
        return movePositions[playerIndex];
    }
}
