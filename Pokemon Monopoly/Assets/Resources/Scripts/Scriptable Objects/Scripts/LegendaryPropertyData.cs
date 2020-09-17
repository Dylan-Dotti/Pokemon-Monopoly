using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LegendaryPropertyData", menuName = "Scriptable Objects/Legendary Property Data")]
public class LegendaryPropertyData : PropertyData
{
    public int RentWithBothLegendaries => rentWithBothLegendaries;

    public override int CurrentRent
    {
        get
        {
            switch (Owner.LegendaryProperties.Count)
            {
                case 0:
                    return 0;
                case 1:
                    return BaseRent;
                case 2:
                    return RentWithBothLegendaries;
                default:
                    throw new System.ArgumentException(
                        "Unexpected legendary count");
            }
        }
    }

    [SerializeField] private int rentWithBothLegendaries = 10;
}
