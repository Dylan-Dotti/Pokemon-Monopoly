using UnityEngine;

public abstract class PropertySquare : BoardSquare, IPurchasable
{
    public abstract PropertyData Property { get; set; }
    public MonopolyPlayer Owner { get; set; }
    public virtual bool Purchasable => Owner == null;

    public virtual void Purchase(MonopolyPlayer purchaser)
    {
        Debug.Log("Purchasing property: " + Property.PropertyName);
        Owner = purchaser;
        Owner.Money -= Property.PurchaseCost;
    }

    public override void OnPlayerEntered(PlayerAvatar player, bool isLastMove)
    {
        if (isLastMove) Purchase(player.Owner);
    }
}
