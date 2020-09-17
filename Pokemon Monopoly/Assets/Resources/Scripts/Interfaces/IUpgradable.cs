
public interface IUpgradable
{
    bool Upgradable { get; }
    bool Downgradable { get; }
    void Upgrade();
    void Downgrade();
}
