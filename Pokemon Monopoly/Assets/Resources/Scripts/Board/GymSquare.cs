using UnityEngine;

[RequireComponent(typeof(GymSquareUpgradesManager))]
public class GymSquare : PropertySquare
{
    private GymSquareUpgradesManager upgradesManager;
    private GymPropertyData gymData;

    public override PropertyData Property
    {
        get => base.Property;
        set
        {
            if (gymData != null)
            {
                gymData.Upgraded -= OnUpgradeLevelChanged;
                gymData.Downgraded -= OnUpgradeLevelChanged;
            }
            base.Property = value;
            gymData = (GymPropertyData)value;
            gymData.Upgraded += OnUpgradeLevelChanged;
            gymData.Downgraded += OnUpgradeLevelChanged;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        upgradesManager = GetComponent<GymSquareUpgradesManager>();
    }

    private void OnUpgradeLevelChanged()
    {
        upgradesManager.UpgradeLevel = gymData.UpgradeLevel;
    }
}
