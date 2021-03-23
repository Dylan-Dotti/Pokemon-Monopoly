using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BoardSquare : MonoBehaviour
{
    private List<PlayerAvatar> occupants;

    public float Width => transform.lossyScale.x;
    public float Height => transform.lossyScale.y;

    public MonopolyBoard ParentBoard { get; set; }
    public IReadOnlyList<PlayerAvatar> Occupants => occupants;
    protected BoardSquareMovePositions MovePositions { get; private set; }

    protected virtual void Awake()
    {
        occupants = new List<PlayerAvatar>();
        MovePositions = transform.Find("Player Positions Holder")
            .GetComponent<BoardSquareMovePositions>();
    }

    public abstract void OnPlayerEntered(MonopolyPlayer player, bool isLastMove);

    public virtual Vector3 GetAvatarMovePosition(PlayerAvatar player, float hoverHeight = 0.5f)
    {
        return MovePositions.GetMovePosition(Occupants, player);
    }

    public virtual Quaternion GetAvatarMoveRotation()
    {
        return transform.rotation;
    }

    public void AddOccupant(PlayerAvatar newOccupant)
    {
        if (!occupants.Contains(newOccupant))
        {
            occupants.Add(newOccupant);
            PositionOccupants();
        }
    }

    public void RemoveOccupant(PlayerAvatar occupant)
    {
        occupants.Remove(occupant);
        PositionOccupants();
    }

    protected virtual void PositionOccupants()
    {
        foreach (PlayerAvatar o in Occupants)
        {
            o.transform.position = MovePositions
                .GetMovePosition(Occupants, o);
        }
    }
}
