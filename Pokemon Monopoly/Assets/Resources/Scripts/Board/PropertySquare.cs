using UnityEngine;

public abstract class PropertySquare : BoardSquare, IPurchasable
{
    public PropertyData Property { get; set; }
    public MonopolyPlayer Owner { get; set; }
    public virtual bool Purchasable => Owner == null;

    public virtual void Purchase(MonopolyPlayer purchaser)
    {
        Debug.Log("Purchasing property: " + Property.PropertyName);
        Owner = purchaser;
        Owner.Money -= Property.PurchaseCost;
    }

    public override void OnPlayerEntered(MonopolyPlayer player, bool isLastMove)
    {
        if (isLastMove)
        {
            if (player != Owner)
            {
                if (Purchasable) player.PurchaseProperty(Property);
                else player.Money -= 0;
            }
        }
    }
}
