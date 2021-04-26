using UnityEngine;

public abstract class PropertyData : ScriptableObject
{
    [SerializeField] protected string propertyName;
    [SerializeField] protected Sprite propertySprite;
    [SerializeField] protected int purchaseCost;
    [SerializeField] protected int baseRent;
    [SerializeField] protected PropertyCollection propCollection;

    public string PropertyName => propertyName;
    public Sprite PropertySprite => propertySprite;
    public int PurchaseCost => purchaseCost;
    public int BaseRent => baseRent;
    public int MortgageValue => PurchaseCost / 2;
    public int UnmortgageCost => MortgageValue + Mathf.RoundToInt(MortgageValue * 0.1f);
    public virtual int TotalDowngradeValue => 0;
    public string CollectionName => CollectionData.CollectionName;

    public PropertyCollection CollectionData
    {
        get => propCollection;
        set => propCollection = value;
    }

    public MonopolyPlayer Owner { get; set; }
    public virtual bool Mortgageable => Owner != null && !IsMortgaged;
    public bool IsMortgaged { get; set; }
    public abstract int CurrentRent { get; }

    public virtual int NumMarts => 0;
    public virtual int NumCenters => 0;

    public abstract void EnablePropertyDisplay(IPropertyDisplay display);
}
