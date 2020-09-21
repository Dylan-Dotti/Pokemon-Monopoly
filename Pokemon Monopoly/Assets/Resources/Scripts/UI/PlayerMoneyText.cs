
public class PlayerMoneyText : PlayerValueText
{
    protected override string GetValueText(MonopolyPlayer player)
    {
        return SpecialCharacters.POKEMONEY_SYMBOL + player.Money.ToString();
    }
}
