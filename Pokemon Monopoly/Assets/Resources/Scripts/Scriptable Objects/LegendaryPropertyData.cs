using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LegendaryPropertyData", menuName = "Scriptable Objects/Legendary Property Data")]
public class LegendaryPropertyData : PropertyData
{
    [SerializeField] private int rentWithBothLegendaries = 10;

    private DiceRoller roller;
     
    public int RentWithBothLegendaries => rentWithBothLegendaries;

    public override int CurrentRent
    {
        get
        {
            if (roller == null) Debug.Log("Roller null");
            //roller = DiceRoller.Instance;
            int rollMultiplier = roller == null || roller.LastRollTotal == 0 ?
                1 : roller.LastRollTotal;
            return CurrentRentWithoutRoll * rollMultiplier;
        }
    }

    public int CurrentRentWithoutRoll
    {
        get
        {
            if (Owner == null) return BaseRent;
            int numLegendsOwned = CollectionData.NumPropsInCollectionOwned(Owner);
            switch (numLegendsOwned)
            {
                case 0:
                    return 0;
                case 1:
                    return BaseRent;
                case 2:
                    return RentWithBothLegendaries;
                default:
                    throw new System.ArgumentException("Unexpected legendary count");
            }
        }
    }

    private void Awake()
    {
        roller = DiceRoller.Instance;
    }

    public override void EnablePropertyDisplay(IPropertyDisplay display)
    {
        display.EnableDisplay(this);
    }
}
