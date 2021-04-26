
public class RivalBattleSquare : BoardSquare
{
    public override void ApplyEffects(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove)
        {
            EventLogger.Instance.LogEventLocal((
                player.IsLocalPlayer ? "You were attacked by your " :
                $"{player.PlayerName} was attacked by their ") +
                "rival and paid " + 200.ToPokeMoneyString());
            player.Money -= 200;
        }
    }
}
