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

    public void MoveToSquare(BoardSquare square, 
        bool isLastMove = false, bool triggerEvents = true)
    {
        AddToSquare(square);
        square.AddOccupant(this);
        transform.position = square.GetPlayerMovePosition(this);
        if (triggerEvents)
        {
            if (isLastMove) FinishedMove?.Invoke(this);
            square.OnPlayerEntered(this, isLastMove);
        }
    }

    public Coroutine LerpToSquare(BoardSquare square,
        float speed = 2, bool isLastMove = true,
        bool triggerEvents = true)
    {
        return StartCoroutine(LerpToSquareCR(
            square, speed, isLastMove, triggerEvents));
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

    private IEnumerator LerpToSquareCR(BoardSquare square,
        float speed, bool isLastMove, bool triggerEvents)
    {
        yield return lerper.SpeedLerp(
            square.GetPlayerMovePosition(this), speed);
        MoveToSquare(square);
    }
}
