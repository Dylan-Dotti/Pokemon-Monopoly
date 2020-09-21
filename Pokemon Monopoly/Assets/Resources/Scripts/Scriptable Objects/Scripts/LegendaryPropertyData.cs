using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LegendaryPropertyData", menuName = "Scriptable Objects/Legendary Property Data")]
public class LegendaryPropertyData : PropertyData
{
    [SerializeField] private int rentWithBothLegendaries = 10;
     
    public int RentWithBothLegendaries => rentWithBothLegendaries;

    public override int CurrentRent
    {
        get
        {
            int numPlayerLegendaries = Owner.Properties
                .Where(p => p.CollectionName == "Legendaries").Count();
            switch (numPlayerLegendaries)
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
}
