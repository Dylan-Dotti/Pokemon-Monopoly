using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GymPropertyData", menuName = "Scriptable Objects/Gym Property Data")]
public sealed class GymPropertyData : PropertyData, IUpgradable
{
    public event UnityAction Upgraded;
    public event UnityAction Downgraded;

    public int UpgradeCost => upgradeCost;
    public int DowngradeValue => UpgradeCost / 2;
    public int RentWithMonopoly => BaseRent * 2;
    public int RentWithCenter => rentWithCenter;
    public GymData Gym => gymData;

    public override int CurrentRent
    {
        get
        {
            if (upgradeLevel >= 1)
            {
                return upgradeLevel == 5 ? RentWithCenter : RentWithMarts(upgradeLevel);
            }
            return CollectionData.PlayerHasMonopoly(Owner) ?
                RentWithMonopoly : BaseRent;

        }
    }

    public override int NumMarts => upgradeLevel < 5 ? upgradeLevel : 0;
    public override int NumCenters =>
        UpgradeLevel == GymSquareUpgradeLevel.OneCenter ? 1 : 0;

    public override bool Mortgageable => base.Mortgageable && upgradeLevel == 0;

    public bool Upgradable => Owner != null && upgradeLevel < 5 && !IsMortgaged &&
        CollectionData.PlayerHasMonopoly(Owner);
    public bool Downgradable => upgradeLevel > 0 && !IsMortgaged;
    public GymSquareUpgradeLevel UpgradeLevel => (GymSquareUpgradeLevel)upgradeLevel;

    [SerializeField] private int upgradeCost;
    [SerializeField] private int[] rentWithMarts = new int[4];
    [SerializeField] private int rentWithCenter;
    [SerializeField] private GymData gymData;

    private int upgradeLevel = 0;

    public int RentWithMarts(int martCount)
    {
        if (martCount < 0)
        {
            throw new System.ArgumentException(
                "Can't have less than 0 marts");
        }
        if (martCount > rentWithMarts.Length)
        {
            throw new System.ArgumentException(
                $"Can't have more than {rentWithMarts.Length} marts");
        }
        if (martCount == 0) return BaseRent;
        return rentWithMarts[martCount - 1];
    }

    public void Upgrade()
    {
        if (!Upgradable) return;
        upgradeLevel++;
        Upgraded?.Invoke();
    }

    public void Downgrade()
    {
        if (!Downgradable) return;
        upgradeLevel--;
        Downgraded?.Invoke();
    }

    public override void EnablePropertyDisplay(IPropertyDisplay display)
    {
        display.EnableDisplay(this);
    }
}

public enum GymSquareUpgradeLevel
{
    None, OneMart, TwoMarts, ThreeMarts, FourMarts, OneCenter
}

public static class GymUpgradeLevelExtensions
{
    public static string ToDisplayString(this GymSquareUpgradeLevel upgradeLevel)
    {
        switch (upgradeLevel)
        {
            case GymSquareUpgradeLevel.None:
                return "None";
            case GymSquareUpgradeLevel.OneMart:
                return "Mart";
            case GymSquareUpgradeLevel.TwoMarts:
                return "Mart x2";
            case GymSquareUpgradeLevel.ThreeMarts:
                return "Mart x3";
            case GymSquareUpgradeLevel.FourMarts:
                return "Mart x4";
            case GymSquareUpgradeLevel.OneCenter:
                return "Center";
            default:
                throw new System.ArgumentException("Unsupported upgrade level");
        }
    }
}
