using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBattleSquare : BoardSquare
{
    public override void OnPlayerEntered(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove) player.Money -= 100;
    }
}
