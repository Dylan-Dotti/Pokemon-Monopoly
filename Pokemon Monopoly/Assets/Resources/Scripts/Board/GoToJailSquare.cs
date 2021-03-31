
public class GoToJailSquare : CornerSquare
{
    public override void ApplyEffects(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove) player.GoToJailLocal();
    }
}
