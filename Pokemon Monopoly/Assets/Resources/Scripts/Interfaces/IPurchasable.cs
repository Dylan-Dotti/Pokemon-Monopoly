
public interface IPurchasable
{
    bool Purchasable { get; }
    void Purchase(MonopolyPlayer purchaser);
}
