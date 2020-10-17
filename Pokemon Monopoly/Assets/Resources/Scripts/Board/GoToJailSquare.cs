
public class GoToJailSquare : CornerSquare
{
    public override void OnPlayerEntered(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove) player.GoToJail(ParentBoard.GetJailSquare());
    }
}
