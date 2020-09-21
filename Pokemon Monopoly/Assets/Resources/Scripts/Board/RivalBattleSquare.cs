
public class RivalBattleSquare : BoardSquare
{
    public override void OnPlayerEntered(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove) player.Money -= 200;
    }
}
