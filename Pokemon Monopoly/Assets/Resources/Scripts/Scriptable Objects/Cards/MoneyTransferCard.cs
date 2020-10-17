using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoneyTransferCard", menuName = "Scriptable Objects/Cards/Money Transfer Card")]
public class MoneyTransferCard : Card
{
    [SerializeField] protected int transferAmount;
    [SerializeField] protected MoneyTransferMethod transferMethod;

    public int TransferAmount => transferAmount;
    public MoneyTransferMethod TransferMethod => transferMethod;

    public override void ApplyEffect(MonopolyPlayer drawingPlayer,
        PlayerManager pManager, MonopolyBoard board)
    {
        switch (transferMethod)
        {
            case MoneyTransferMethod.FromBank:
                drawingPlayer.Money += transferAmount;
                break;
            case MoneyTransferMethod.ToBank:
                drawingPlayer.Money -= transferAmount;
                break;
            case MoneyTransferMethod.FromAllPlayers:
                foreach (MonopolyPlayer opponent in pManager.GetOpponents(drawingPlayer))
                {
                    opponent.Money -= transferAmount;
                    drawingPlayer.Money += transferAmount;
                }
                break;
            case MoneyTransferMethod.ToAllPlayers:
                foreach (MonopolyPlayer opponent in pManager.GetOpponents(drawingPlayer))
                {
                    drawingPlayer.Money -= transferAmount;
                    opponent.Money += transferAmount;
                }
                break;
            default:
                throw new System.ArgumentException("Invalid transfer method");
        }
    }
}

public enum MoneyTransferMethod
{
    FromBank, ToBank, FromAllPlayers, ToAllPlayers
}
