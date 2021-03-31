
public class TrainerBattleSquare : CardSquare
{
    public override void ApplyEffects(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove)
        {
            Card tbCard = ParentBoard.DrawTrainerBattleCard(player);
            EventLogger.Instance.LogEventLocal(tbCard.Description);
        }
    }
}
