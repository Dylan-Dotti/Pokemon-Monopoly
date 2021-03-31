using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Vector3Lerper))]
public class PlayerAvatar : MonoBehaviour
{
    public event UnityAction<PlayerAvatar> StartedSequentialMove;
    public event UnityAction<PlayerAvatar> FinishedSequentialMove;
    public event UnityAction<PlayerAvatar, BoardSquare> MovedToSquare;

    private MonopolyPlayer owner;
    private Vector3Lerper lerper;

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
        lerper = GetComponent<Vector3Lerper>();
    }

    public void SpawnAtSquare(BoardSquare square)
    {
        MoveToSquare(square, triggerEvents:false);
    }

    public void MoveToPosition(Vector3 position)
    {
        transform.position = position;
    }

    public Coroutine LerpToPosition(Vector3 position)
    {
        return lerper.DurationLerp(transform.position, position, .165f,
            vector => transform.position = vector);
    }

    public void MoveToSquare(BoardSquare square, 
        bool isLastMove = false, bool triggerEvents = true)
    {
        AddToSquare(square);
        transform.position = square.GetAvatarMovePosition(this);
        transform.rotation = square.GetAvatarMoveRotation();
        if (isLastMove) MovedToSquare?.Invoke(this, square);
        if (triggerEvents) square.ApplyEffects(Owner, isLastMove);
    }

    public Coroutine LerpToSquare(BoardSquare square,
        bool isLastMove = true, bool triggerEvents = true,
        bool hideDuringMove = false)
    {
        AddToSquare(square);
        return StartCoroutine(LerpToSquareCR(
            square, .165f, isLastMove, triggerEvents, hideDuringMove));
    }

    public Coroutine MoveSequential(IReadOnlyList<BoardSquare> squareSequence,
        float interval, bool triggerEvents = true)
    {
        return StartCoroutine(MoveSequentialCR(squareSequence, 0.335f));
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
        if (square == OccupiedSquare)
        {
            square.PositionOccupants();
        }
        else
        {
            RemoveFromSquare(OccupiedSquare);
            OccupiedSquare = square;
            square.AddOccupant(this);
        }
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
        //float moveInterval = interval * .33f;
        //float waitInterval = interval * .67f;
        for (int i = 0; i < squares.Count - 1; i++)
        {
            yield return new WaitForSeconds(interval);
            //MoveToSquare(squares[i]);
            yield return LerpToSquare(
                squares[i], false, false);
        }
        yield return new WaitForSeconds(interval);
        //MoveToSquare(squares[squares.Count - 1], isLastMove: true, triggerEvents: false);
        yield return LerpToSquare(
            squares[squares.Count - 1], true, false);
        yield return new WaitForSeconds(interval);
        // Invoke event before triggering square events in case of multiple sequential moves
        FinishedSequentialMove?.Invoke(this);
        //MoveToSquare(squares[squares.Count - 1], isLastMove: true, triggerEvents: true);
        yield return LerpToSquare(
            squares[squares.Count - 1], true, true);
    }

    private IEnumerator LerpToSquareCR(BoardSquare square,
        float duration, bool isLastMove, bool triggerEvents,
        bool hideDuringMove)
    {
        //if (hideDuringMove) gameObject.SetActive(false);
        lerper.DurationLerp(
            transform.eulerAngles, square.GetAvatarMoveRotation().eulerAngles,
            duration,
            vector => transform.rotation = Quaternion.Euler(vector));
        yield return lerper.DurationLerp(
            transform.position, square.GetAvatarMovePosition(this), duration,
            vector => transform.position = vector);
        MoveToSquare(square, isLastMove, triggerEvents);
        gameObject.SetActive(true);
    }
}
