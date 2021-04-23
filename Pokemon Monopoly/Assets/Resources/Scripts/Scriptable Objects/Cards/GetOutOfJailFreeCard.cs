using UnityEngine;

[CreateAssetMenu(fileName = "Get out of Jail Card", menuName = "Scriptable Objects/Cards/Get out of Jail Card")]
public class GetOutOfJailFreeCard : Card
{
    public override void ApplyEffect(MonopolyPlayer drawingPlayer,
        PlayerManager pManager, MonopolyBoard board)
    {
        drawingPlayer.AddGetOutOfJailFreeUse();
    }
}
