using UnityEngine.Events;

public interface IUpgradable
{
    event UnityAction Upgraded;
    event UnityAction Downgraded;

    int UpgradeCost { get; }
    int DowngradeValue { get; }

    bool Upgradable { get; }
    bool Downgradable { get; }

    void Upgrade();
    void Downgrade();
}
