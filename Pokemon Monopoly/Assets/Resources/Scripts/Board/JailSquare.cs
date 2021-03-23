using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JailSquare : CornerSquare
{
    private BoardSquareMovePositions jailMovePositions;

    protected override void Awake()
    {
        base.Awake();
        jailMovePositions = transform.Find("Player Jail Positions Holder")
            .GetComponent<BoardSquareMovePositions>();
    }

    public override void OnPlayerEntered(MonopolyPlayer player, bool isLastMove)
    {

    }

    public override Vector3 GetAvatarMovePosition(PlayerAvatar player, float hoverHeight = 0.5F)
    {
        return player.Owner.InJail ?
            jailMovePositions.GetMovePosition(Occupants.Where(o => o.Owner.InJail).ToList(), player):
            MovePositions.GetMovePosition(Occupants.Where(o => !o.Owner.InJail).ToList(), player);
    }

    protected override void PositionOccupants()
    {
        var jailOccupants = Occupants.Where(o => o.Owner.InJail).ToList();
        var nonJailOccupants = Occupants.Where(o => !o.Owner.InJail).ToList();
        jailOccupants.ForEach(
            o => o.transform.position = jailMovePositions
            .GetMovePosition(jailOccupants, o));
        nonJailOccupants.ForEach(
            o => o.transform.position = MovePositions
            .GetMovePosition(nonJailOccupants, o));
    }
}
