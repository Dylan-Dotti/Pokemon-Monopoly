using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GymPropertyData", menuName = "Scriptable Objects/Gym Property Data")]
public sealed class GymPropertyData : PropertyData, IUpgradable
{
    public int UpgradeCost => upgradeCost;
    public int RentWithCenter => rentWithCenter;
    public GymData Gym => gymData;

    public override int CurrentRent => upgradeLevel <= 4 ?
        RentWithMarts(upgradeLevel) : BaseRent;

    public bool Upgradable => upgradeLevel < 5;
    public bool Downgradable => upgradeLevel > 0;

    [SerializeField] private int upgradeCost;
    [SerializeField] private int[] rentWithMarts = new int[4];
    [SerializeField] private int rentWithCenter;
    [SerializeField] private GymData gymData;

    private int upgradeLevel;

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
    }

    public void Downgrade()
    {
        if (!Downgradable) return;
        upgradeLevel--;
    }
}
