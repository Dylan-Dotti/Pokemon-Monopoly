
public class PlayerNameText : PlayerValueText
{
    protected override string GetValueText(MonopolyPlayer player)
    {
        return player.PlayerName;
    }
}
