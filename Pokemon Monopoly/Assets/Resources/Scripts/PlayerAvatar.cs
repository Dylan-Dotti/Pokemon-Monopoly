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

    [SerializeField] private GameObject avatarGraphics;

    private MonopolyPlayer owner;
    private Vector3Lerper positionLerper;

    private AvatarImageFactory imageFactory;
    private Transform avatarImageHolder;
    private Transform avatarImage;

    public MonopolyPlayer Owner
    {
        get => owner;
        set
        {
            owner = value;
            if (owner != null)
            {
                SpawnOwnerAvatarImage();
            }
        }
    }

    public BoardSquare OccupiedSquare { get; private set; }
    public bool InJailOverride { get; set; }

    private void Awake()
    {
        avatarImageHolder = transform.Find("Canvas");
        positionLerper = GetComponent<Vector3Lerper>();
        imageFactory = AvatarImageFactory.Instance;
    }

    public void SpawnAtSquare(BoardSquare square)
    {
        MoveToSquare(square, triggerEvents:false);
    }

    public void Despawn()
    {
        RemoveFromSquare();
        gameObject?.SetActive(false);
    }

    public void MoveToPosition(Vector3 position)
    {
        transform.position = position;
    }

    public Coroutine LerpToPosition(Vector3 position)
    {
        Vector3Lerp lerp = new Vector3Lerp(
            transform.position, position, vector => transform.position = vector);
        return positionLerper.DurationLerp(lerp, .165f);
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
        bool isLastMove = true, bool triggerEffects = true,
        bool hideDuringMove = false)
    {
        AddToSquare(square);
        return StartCoroutine(LerpToSquareCR(
            square, .165f, isLastMove, triggerEffects, hideDuringMove));
    }

    public void SpawnAvatarImage(string avatarImageName)
    {
        if (avatarImageHolder.childCount == 2)
        {
            Destroy(avatarImageHolder.GetChild(1).gameObject);
        }
        imageFactory.SpawnAvatarImage(
            avatarImageName, avatarImageHolder, new Vector3(.005f, .005f, 1));
    }

    private void SpawnOwnerAvatarImage()
    {
        SpawnAvatarImage(Owner.AvatarImageName);
    }

    private void AddToSquare(BoardSquare square)
    {
        if (square == OccupiedSquare)
        {
            square.PositionOccupants();
        }
        else
        {
            RemoveFromSquare();
            OccupiedSquare = square;
            square.AddOccupant(this);
        }
    }

    public void RemoveFromSquare()
    {
        if (OccupiedSquare != null)
        {
            OccupiedSquare.RemoveOccupant(this);
            OccupiedSquare = null;
        }
    }

    private IEnumerator LerpToSquareCR(BoardSquare square,
        float duration, bool isLastMove, bool triggerEffects,
        bool hideDuringMove)
    {
        if (hideDuringMove) avatarGraphics.SetActive(false);
        Vector3 startRotation = transform.eulerAngles;
        Vector3 endRotation = square.GetAvatarMoveRotation().eulerAngles;
        (float startDegrees, float endDegrees) = 
            RotationNormalizer.Normalize(startRotation.z, endRotation.z);
        startRotation = new Vector3(startRotation.x, startRotation.y, startDegrees);
        endRotation = new Vector3(endRotation.x, endRotation.y, endDegrees);
        var lerps = new List<Vector3Lerp>
        {
            new Vector3Lerp(
                transform.position, square.GetAvatarMovePosition(this),
                vector => transform.position = vector),
            new Vector3Lerp(
                startRotation, endRotation,
                vector => transform.rotation = Quaternion.Euler(vector))
        };
        yield return positionLerper.MultiDurationLerp(lerps, duration);
        avatarGraphics.SetActive(true);
        if (triggerEffects) square.ApplyEffects(owner, isLastMove);
    }
}
