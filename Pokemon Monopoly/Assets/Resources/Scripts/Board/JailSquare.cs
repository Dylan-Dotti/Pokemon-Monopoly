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

    public override void ApplyEffects(MonopolyPlayer player, bool isLastMove)
    {

    }

    public override Vector3 GetAvatarMovePosition(PlayerAvatar player, float hoverHeight = 0.5F)
    {
        return player.Owner.InJail ?
            jailMovePositions.GetMovePosition(Occupants.Where(o => o.Owner.InJail).ToList(), player):
            MovePositions.GetMovePosition(Occupants.Where(o => !o.Owner.InJail).ToList(), player);
    }

    public override void PositionOccupants(PlayerAvatar ignorePlayer = null)
    {
        var jailOccupants = Occupants.Where(o => o.Owner.InJail).ToList();
        var nonJailOccupants = Occupants.Where(o => !o.Owner.InJail).ToList();
        var jailOccupantsFiltered = jailOccupants.Where(
            o => ignorePlayer == null || o != ignorePlayer).ToList();
        var nonJailOccupantsFiltered = nonJailOccupants.Where(
            o => ignorePlayer == null || !o.Owner.InJail).ToList();
        jailOccupantsFiltered.ForEach(o => o.LerpToPosition(
            jailMovePositions.GetMovePosition(jailOccupants, o)));
        nonJailOccupantsFiltered.ForEach(o => o.LerpToPosition(
            MovePositions.GetMovePosition(nonJailOccupants, o)));
    }
}
