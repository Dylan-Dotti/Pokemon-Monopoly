using System;
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

    public override int TotalDowngradeValue => DowngradeValue *
        (UpgradeLevel == GymPropertyUpgradeLevel.OneCenter ? 5 : NumMarts);

    public override int NumMarts => upgradeLevel < 5 ? upgradeLevel : 0;
    public override int NumCenters =>
        UpgradeLevel == GymPropertyUpgradeLevel.OneCenter ? 1 : 0;

    public override bool Mortgageable => base.Mortgageable && upgradeLevel == 0;

    public bool Upgradable => Owner != null && upgradeLevel < 5 && !IsMortgaged &&
        CollectionData.PlayerHasMonopoly(Owner);

    public bool Downgradable => upgradeLevel > 0 && !IsMortgaged;

    public GymPropertyUpgradeLevel UpgradeLevel => (GymPropertyUpgradeLevel)upgradeLevel;

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

    public bool IsUpgradable(BuildingsManager buildingManager)
    {
        return Owner != null && upgradeLevel < 5 && !IsMortgaged &&
            CanTakeUpgrade(buildingManager) &&
            CollectionData.PlayerHasMonopoly(Owner);
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

    private bool CanTakeUpgrade(BuildingsManager buildingsManager)
    {
        switch (UpgradeLevel)
        {
            case GymPropertyUpgradeLevel.None:
            case GymPropertyUpgradeLevel.OneMart:
            case GymPropertyUpgradeLevel.TwoMarts:
            case GymPropertyUpgradeLevel.ThreeMarts:
                return buildingsManager.CanGetMart;
            case GymPropertyUpgradeLevel.FourMarts:
                return buildingsManager.CanGetCenter;
            case GymPropertyUpgradeLevel.OneCenter:
                return false;
            default:
                throw new ArgumentException("Invalid upgrade level");
        }
    }
}

public enum GymPropertyUpgradeLevel
{
    None, OneMart, TwoMarts, ThreeMarts, FourMarts, OneCenter
}

public static class GymUpgradeLevelExtensions
{
    public static string ToDisplayString(this GymPropertyUpgradeLevel upgradeLevel)
    {
        switch (upgradeLevel)
        {
            case GymPropertyUpgradeLevel.None:
                return "None";
            case GymPropertyUpgradeLevel.OneMart:
                return "Mart";
            case GymPropertyUpgradeLevel.TwoMarts:
                return "Mart x2";
            case GymPropertyUpgradeLevel.ThreeMarts:
                return "Mart x3";
            case GymPropertyUpgradeLevel.FourMarts:
                return "Mart x4";
            case GymPropertyUpgradeLevel.OneCenter:
                return "Center";
            default:
                throw new System.ArgumentException("Unsupported upgrade level");
        }
    }
}
