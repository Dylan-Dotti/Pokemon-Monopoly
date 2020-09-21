
public class GoSquare : BoardSquare
{
    public override void OnPlayerEntered(MonopolyPlayer player, bool isLastMove)
    {
        player.Money += 200;
    }
}
