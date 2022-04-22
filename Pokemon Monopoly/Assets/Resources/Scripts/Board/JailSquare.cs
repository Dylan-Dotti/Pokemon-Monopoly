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

    public override void ApplyEffects(MonopolyPlayer player, bool isLastMove) { }

    public override Vector3 GetAvatarMovePosition(PlayerAvatar player, float hoverHeight = 0.5F)
    {
        return IsJailable(player) ?
            jailMovePositions.GetMovePosition(Occupants.Where(o => IsJailable(o)).ToList(), player) :
            MovePositions.GetMovePosition(Occupants.Where(o => !IsJailable(o)).ToList(), player);
    }

    public override void PositionOccupants(PlayerAvatar ignorePlayer = null)
    {
        var jailOccupants = Occupants.Where(o => IsJailable(o)).ToList();
        var nonJailOccupants = Occupants.Where(o => !IsJailable(o)).ToList();

        var jailOccupantsFiltered = jailOccupants.Where(
            occ => ignorePlayer == null || occ != ignorePlayer).ToList();
        var nonJailOccupantsFiltered = nonJailOccupants.Where(
            occ => ignorePlayer == null || !IsJailable(occ)).ToList();

        jailOccupantsFiltered.ForEach(o => o.LerpToPosition(
            jailMovePositions.GetMovePosition(jailOccupants, o)));
        nonJailOccupantsFiltered.ForEach(o => o.LerpToPosition(
            MovePositions.GetMovePosition(nonJailOccupants, o)));
    }

    private bool IsJailable(PlayerAvatar avatar)
    {
        return (avatar.Owner != null && avatar.Owner.InJail) || avatar.InJailOverride;
    }
}
