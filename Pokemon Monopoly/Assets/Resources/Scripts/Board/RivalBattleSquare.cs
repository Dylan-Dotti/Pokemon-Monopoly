
public class RivalBattleSquare : BoardSquare
{
    public override void OnPlayerEntered(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove)
        {
            player.Money -= 200;

            EventLogger.Instance.LogEventLocal((
                player.IsLocalPlayer ? "You were attacked by your " :
                $"{player.PlayerName} was attacked by their ") +
                "rival and paid " + 200.ToPokeMoneyString());
        }
    }
}
