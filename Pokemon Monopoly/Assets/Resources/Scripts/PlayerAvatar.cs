using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PositionLerper))]
public class PlayerAvatar : MonoBehaviour
{
    public event UnityAction<PlayerAvatar> FinishedMove;
    private PositionLerper lerper;

    public BoardSquare OccupiedSquare { get; private set; }

    private void Awake()
    {
        lerper = GetComponent<PositionLerper>();
    }

    public void SpawnAtSquare(BoardSquare square)
    {
        MoveToSquare(square, triggerEvents:false);
    }

    public void MoveToSquare(
        BoardSquare square, bool isLastMove = true,
        bool triggerEvents = true)
    {
        AddToSquare(square);
        square.AddOccupant(this);
        transform.position = square.GetPlayerMovePosition(this);
        if (triggerEvents) square.OnPlayerEntered(this, isLastMove);
    }

    private void AddToSquare(BoardSquare square)
    {
        RemoveFromSquare(OccupiedSquare);
        OccupiedSquare = square;
        square.AddOccupant(this);
    }

    private void RemoveFromSquare(BoardSquare square)
    {
        if (OccupiedSquare != null)
        {
            OccupiedSquare.RemoveOccupant(this);
            OccupiedSquare = null;
        }
    }
}
