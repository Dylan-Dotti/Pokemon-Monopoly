using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoSquare : BoardSquare
{
    public override void OnPlayerEntered(PlayerAvatar player, bool isLastMove)
    {
        player.Owner.Money += 200;
    }
}
