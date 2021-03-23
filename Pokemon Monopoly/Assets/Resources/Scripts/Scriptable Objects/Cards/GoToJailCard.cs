using UnityEngine;

[CreateAssetMenu(fileName = "Go to Jail Card", menuName = "Scriptable Objects/Cards/Go to Jail Card")]
public class GoToJailCard : Card
{
    public override void ApplyEffect(MonopolyPlayer drawingPlayer, 
        PlayerManager pManager, MonopolyBoard board)
    {
        drawingPlayer.GoToJailLocal();
    }
}
