
public class RocketBattleSquare : BoardSquare
{
    public override void ApplyEffects(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove)
        {
            EventLogger.Instance.LogEventLocal((
                player.IsLocalPlayer ? "You were " : $"{player.PlayerName} was ") +
                "attacked by Team Rocket and paid " + 100.ToPokeMoneyString());
            player.Money -= 100;
        }
    }
}
