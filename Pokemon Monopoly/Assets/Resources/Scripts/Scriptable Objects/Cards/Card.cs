using UnityEngine;

public abstract class Card : ScriptableObject
{
    [SerializeField] protected Popup popupPrefab;
    [SerializeField] protected string description;

    public string Description => description;

    public abstract void ApplyEffect(MonopolyPlayer drawingPlayer,
        PlayerManager pManager, MonopolyBoard board);
}
