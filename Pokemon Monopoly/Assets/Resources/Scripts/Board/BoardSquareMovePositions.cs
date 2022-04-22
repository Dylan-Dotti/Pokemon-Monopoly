using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardSquareMovePositions : MonoBehaviour
{
    private Vector3[][] playerMovePositions;

    public int MaxMovePositions => playerMovePositions.Length;

    private void Awake()
    {
        playerMovePositions = new Vector3[transform.childCount][];
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform positionsParent = transform.GetChild(i);
            playerMovePositions[i] = new Vector3[positionsParent.childCount];
            for (int j = 0; j < positionsParent.childCount; j++)
            {
                playerMovePositions[i][j] = positionsParent.GetChild(j).position;
            }
        }
    }

    public IReadOnlyDictionary<PlayerAvatar, Vector3> GetMovePositions(
        IReadOnlyList<PlayerAvatar> players)
    {
        return players.ToDictionary(p => p, p => GetMovePosition(players, p));
    }

    public Vector3 GetMovePosition(IReadOnlyList<PlayerAvatar> currentPlayers,
        PlayerAvatar movingPlayer)
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
        if (numPlayers > MaxMovePositions)
        {
            throw new System.IndexOutOfRangeException(
                $"More than {MaxMovePositions} avatars are not supported for this square");
        }
        Vector3[] movePositions = playerMovePositions[numPlayers - 1];
        return movePositions[playerIndex];
    }
}
