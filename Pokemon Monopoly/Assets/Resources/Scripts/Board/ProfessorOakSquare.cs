
public class ProfessorOakSquare : CardSquare
{
    public override void ApplyEffects(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove)
        {
            Card tbCard = ParentBoard.DrawProfessorCard(player);
            EventLogger.Instance.LogEventLocal(tbCard.Description);
        }
    }
}
