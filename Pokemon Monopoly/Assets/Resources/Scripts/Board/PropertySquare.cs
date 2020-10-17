
public abstract class PropertySquare : BoardSquare
{
    public virtual PropertyData Property { get; set; }

    private PopupManager popupManager;

    protected override void Awake()
    {
        base.Awake();
        popupManager = PopupManager.Instance;
    }

    public override void OnPlayerEntered(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove && player.IsLocalPlayer && Property != null)
        {
            if (Property.Owner == null)
            {
                popupManager.QueuePopup(
                    popupManager.Factory.GetPropertyPurchasePrompt(player, Property),
                    PopupOpenOptions.Queue, true);
            }
            else if (Property.Owner != player && !Property.IsMortgaged)
            {
                player.PayRent(Property);
            }
        }
    }
}
