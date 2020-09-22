
public class GoSquare : CornerSquare
{
    public override void OnPlayerEntered(MonopolyPlayer player, bool isLastMove)
    {
        player.Money += 200;
    }
}
