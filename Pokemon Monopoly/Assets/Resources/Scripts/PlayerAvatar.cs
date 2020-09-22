using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PositionLerper))]
public class PlayerAvatar : MonoBehaviour
{
    public event UnityAction<PlayerAvatar> FinishedMove;

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
        transform.position = square.GetPlayerMovePosition(this);
        transform.rotation = square.GetPlayerMoveRotation();
        if (triggerEvents)
        {
            if (isLastMove) FinishedMove?.Invoke(this);
            square.OnPlayerEntered(Owner, isLastMove);
        }
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
        Instantiate(AvatarImageFactory.Instance.GetAvatarImage(
            Owner.AvatarImageName, canvas, new Vector3(.005f, .005f, 1)));
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
