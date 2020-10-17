
public class PlayerMoneyText : PlayerValueTextPrefixed
{
    protected override string GetValueText(MonopolyPlayer player)
    {
        return base.GetValueText(player) + player.Money.ToPokeMoneyString();
    }
}
