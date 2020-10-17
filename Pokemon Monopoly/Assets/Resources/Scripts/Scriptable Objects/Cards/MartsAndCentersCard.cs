using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Marts and Centers Card", menuName = "Scriptable Objects/Cards/Marts and Centers Card")]
public sealed class MartsAndCentersCard : Card
{
    [SerializeField] private int costPerMart;
    [SerializeField] private int costPerCenter;

    public override void ApplyEffect(MonopolyPlayer drawingPlayer,
        PlayerManager pManager, MonopolyBoard board)
    {
        var properties = drawingPlayer.Properties;
        int numMarts = properties.Select(p => p.NumMarts).Sum();
        int numCenters = properties.Select(p => p.NumCenters).Sum();
        int totalCost = numMarts * costPerMart + numCenters * costPerCenter;
        drawingPlayer.Money -= totalCost;
    }
}
