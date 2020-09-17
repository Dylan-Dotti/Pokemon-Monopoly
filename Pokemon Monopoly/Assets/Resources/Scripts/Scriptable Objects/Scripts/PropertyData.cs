using UnityEngine;

public abstract class PropertyData : ScriptableObject
{
    public string PropertyName => propertyName;
    public Sprite PropertySprite => propertySprite;
    public int PurchaseCost => purchaseCost;
    public int BaseRent => baseRent;
    public int MortgageValue => PurchaseCost / 2;
    public PropertyCollection CollectionData => propCollection;

    public MonopolyPlayer Owner { get; set; }
    public abstract int CurrentRent { get; }

    [SerializeField] protected string propertyName;
    [SerializeField] protected Sprite propertySprite;
    [SerializeField] protected int purchaseCost;
    [SerializeField] protected int baseRent;
    [SerializeField] protected PropertyCollection propCollection;
}
