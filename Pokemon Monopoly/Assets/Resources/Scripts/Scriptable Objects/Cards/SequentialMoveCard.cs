using UnityEngine;

[CreateAssetMenu(fileName = "SequentialMoveCard", menuName = "Scriptable Objects/Cards/Sequential Move Card")]
public class SequentialMoveCard : Card
{
    [SerializeField] private int numSpaces;
    [SerializeField] private bool reversed;

    public override void ApplyEffect(MonopolyPlayer drawingPlayer,
        PlayerManager pManager, MonopolyBoard board)
    {
        drawingPlayer.MoveAvatarSequentialLocal(numSpaces, reversed);
    }
}
