using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BoardSquare : MonoBehaviour
{
    public float Width => transform.lossyScale.x;
    public float Height => transform.lossyScale.y;

    public HashSet<MonopolyPlayer> Occupants { get; private set; }

    protected virtual void Awake()
    {
        Occupants = new HashSet<MonopolyPlayer>();
    }

    public abstract void OnPlayerEntered(MonopolyPlayer player, bool isLastMove);

    public virtual Vector3 GetPlayerMovePosition(MonopolyPlayer player, float hoverHeight = 0.5f)
    {
        return transform.position + Vector3.back * hoverHeight;
    }

    public void AddOccupant(MonopolyPlayer newOccupant)
    {
        Occupants.Add(newOccupant);
    }

    public void RemoveOccupant(MonopolyPlayer occupant)
    {
        Occupants.Remove(occupant);
    }
}
