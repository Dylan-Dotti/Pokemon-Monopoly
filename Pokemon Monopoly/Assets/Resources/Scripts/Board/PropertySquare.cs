
public abstract class PropertySquare : BoardSquare
{
    public virtual PropertyData Property { get; set; }

    private PopupSpawner popupSpawner;

    protected override void Awake()
    {
        base.Awake();
        popupSpawner = PopupSpawner.Instance;
    }

    public override void ApplyEffects(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove && player.IsLocalPlayer && Property != null)
        {
            if (Property.Owner == null)
            {
                popupSpawner.OpenPropertyPurchasePrompt(player, Property);
            }
            else if (Property.Owner != player && !Property.IsMortgaged)
            {
                player.PayRentAllClients(Property);
            }
        }
    }
}
