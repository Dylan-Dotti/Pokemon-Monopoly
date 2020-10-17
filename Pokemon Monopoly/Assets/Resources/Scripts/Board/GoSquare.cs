
public class GoSquare : CornerSquare
{
    public override void OnPlayerEntered(MonopolyPlayer player, bool isLastMove)
    {
        player.Money += 200;
        EventLogger.Instance.LogEventLocal((
            player.IsLocalPlayer ? "You" : player.PlayerName) +
            " passed Go and collected " + 200.ToPokeMoneyString());
    }
}
