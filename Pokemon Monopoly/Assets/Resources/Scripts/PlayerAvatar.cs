using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PositionLerper))]
public class PlayerAvatar : MonoBehaviour
{
    public event UnityAction<PlayerAvatar> StartedSequentialMove;
    public event UnityAction<PlayerAvatar> FinishedSequentialMove;
    public event UnityAction<PlayerAvatar, BoardSquare> MovedToSquare;

    private MonopolyPlayer owner;
    private PositionLerper lerper;

    public MonopolyPlayer Owner
    {
        get => owner;
        set
        {
            owner = value;
            if (owner != null)
            {
                SpawnAvatarImage();
            }
        }
    }
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
        transform.position = square.GetAvatarMovePosition(this);
        transform.rotation = square.GetAvatarMoveRotation();
        if (isLastMove) MovedToSquare?.Invoke(this, square);
        if (triggerEvents)
        {
            square.OnPlayerEntered(Owner, isLastMove);
        }
    }

    public Coroutine MoveSequential(IReadOnlyList<BoardSquare> squareSequence,
        float interval, bool triggerEvents = true)
    {
        return StartCoroutine(MoveSequentialCR(squareSequence, 0.5f));
    }

    public Coroutine LerpToSquare(BoardSquare square,
        float speed = 2, bool isLastMove = true,
        bool triggerEvents = true)
    {
        return StartCoroutine(LerpToSquareCR(
            square, speed, isLastMove, triggerEvents));
    }

    private void SpawnAvatarImage()
    {
        Transform canvas = transform.Find("Canvas");
        if (canvas.childCount == 2)
        {
            Destroy(canvas.GetChild(1).gameObject);
        }
        Owner.GetNewAvatarImage(canvas, new Vector3(.005f, .005f, 1));
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

    private IEnumerator MoveSequentialCR(
        IReadOnlyList<BoardSquare> squares, float interval, bool triggerEvents = true)
    {
        StartedSequentialMove?.Invoke(this);
        for (int i = 0; i < squares.Count - 1; i++)
        {
            yield return new WaitForSeconds(interval);
            MoveToSquare(squares[i]);
        }
        yield return new WaitForSeconds(interval);
        MoveToSquare(squares[squares.Count - 1], isLastMove: true, triggerEvents: false);
        yield return new WaitForSeconds(interval);
        // Invoke event before triggering square events in case of multiple sequential moves
        FinishedSequentialMove?.Invoke(this);
        MoveToSquare(squares[squares.Count - 1], isLastMove: true, triggerEvents: true);
    }

    private IEnumerator LerpToSquareCR(BoardSquare square,
        float speed, bool isLastMove, bool triggerEvents)
    {
        yield return lerper.SpeedLerp(
            square.GetAvatarMovePosition(this), speed);
        MoveToSquare(square);
    }
}
