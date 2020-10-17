using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoardSquareMovePositions))]
public abstract class BoardSquare : MonoBehaviour
{
    private List<PlayerAvatar> occupants;
    private BoardSquareMovePositions movePositions;

    public float Width => transform.lossyScale.x;
    public float Height => transform.lossyScale.y;

    public IReadOnlyList<PlayerAvatar> Occupants => occupants;
    public MonopolyBoard ParentBoard { get; set; }

    protected virtual void Awake()
    {
        occupants = new List<PlayerAvatar>();
        movePositions = GetComponent<BoardSquareMovePositions>();
    }

    public abstract void OnPlayerEntered(MonopolyPlayer player, bool isLastMove);

    public virtual Vector3 GetPlayerMovePosition(PlayerAvatar player, float hoverHeight = 0.5f)
    {
        return movePositions.GetMovePosition(Occupants, player);
    }

    public virtual Quaternion GetPlayerMoveRotation()
    {
        return transform.rotation;
    }

    public void AddOccupant(PlayerAvatar newOccupant)
    {
        if (!occupants.Contains(newOccupant))
        {
            occupants.Add(newOccupant);
            foreach (var o in Occupants.Where(o => o != newOccupant))
            {
                o.transform.position = movePositions.GetMovePosition(
                    Occupants, o);
            }
        }
    }

    public void RemoveOccupant(PlayerAvatar occupant)
    {
        occupants.Remove(occupant);
        occupants.ForEach(o => o.transform.position = 
            movePositions.GetMovePosition(occupants, o));
    }
}
