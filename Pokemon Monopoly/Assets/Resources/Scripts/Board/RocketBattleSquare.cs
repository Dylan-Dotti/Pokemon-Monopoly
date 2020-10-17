
public class RocketBattleSquare : BoardSquare
{
    public override void OnPlayerEntered(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove)
        {
            player.Money -= 100;
            EventLogger.Instance.LogEventLocal((
                player.IsLocalPlayer ? "You were " : $"{player.PlayerName} was ") +
                "attacked by Team Rocket and paid " + 100.ToPokeMoneyString());
        }
    }
}
