using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardSquare : MonoBehaviour
{
    public float Width => transform.lossyScale.x;
    public float Height => transform.lossyScale.y;

    public HashSet<PlayerAvatar> Occupants { get; private set; }

    protected virtual void Awake()
    {
        Occupants = new HashSet<PlayerAvatar>();
    }

    public abstract void OnPlayerEntered(PlayerAvatar player, bool isLastMove);

    public virtual Vector3 GetPlayerMovePosition(PlayerAvatar player, float hoverHeight = 0.5f)
    {
        return transform.position + Vector3.back * hoverHeight;
    }

    public void AddOccupant(PlayerAvatar newOccupant)
    {
        Occupants.Add(newOccupant);
    }

    public void RemoveOccupant(PlayerAvatar occupant)
    {
        Occupants.Remove(occupant);
    }
}
