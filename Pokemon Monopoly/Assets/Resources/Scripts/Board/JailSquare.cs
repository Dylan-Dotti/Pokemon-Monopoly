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
        //refactor to lerp
        // addtosquare not called, so no resposition happens
        var occupantsFiltered = ignorePlayer == null ?
            Occupants : Occupants.Where(o => o != ignorePlayer);
        var jailOccupants = occupantsFiltered.Where(o => o.Owner.InJail).ToList();
        var nonJailOccupants = occupantsFiltered.Where(o => !o.Owner.InJail).ToList();
        jailOccupants.ForEach(
            o => o.LerpToPosition(jailMovePositions.GetMovePosition(jailOccupants, o)));
        nonJailOccupants.ForEach(
            o => o.LerpToPosition(MovePositions.GetMovePosition(nonJailOccupants, o)));
    }
}
