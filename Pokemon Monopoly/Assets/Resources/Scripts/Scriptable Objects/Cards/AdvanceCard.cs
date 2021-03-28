using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "AdvanceCard", menuName = "Scriptable Objects/Cards/Advance Card")]
public class AdvanceCard : Card
{
    [SerializeField] protected int[] targetSquareIndexes;

    public override void ApplyEffect(MonopolyPlayer drawingPlayer,
        PlayerManager pManager, MonopolyBoard board)
    {
        BoardSquare startSquare = drawingPlayer.Avatar.OccupiedSquare;
        var paths = targetSquareIndexes.Select(
            i => board.GetPathTo(startSquare, board.GetSquareAt(i)));
        var shortestPath = paths.OrderBy(p => p.Count).First();
        drawingPlayer.MoveAvatarSequentialLocal(shortestPath.Count);
    }
}
