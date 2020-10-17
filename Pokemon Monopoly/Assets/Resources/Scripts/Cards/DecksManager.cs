using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardFactory))]
public class DecksManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private MonopolyBoard board;

    private Deck professorDeck;
    private Deck battleDeck;

    private CardFactory cardFactory;

    private void Awake()
    {
        cardFactory = GetComponent<CardFactory>();
        professorDeck = transform.Find("Professor Deck").GetComponent<Deck>();
        battleDeck = transform.Find("Battle Deck").GetComponent<Deck>();
    }

    public void SpawnDecks()
    {
        professorDeck.SpawnDeck(cardFactory.GetProfessorCardSet());
        battleDeck.SpawnDeck(cardFactory.GetTrainerBattleCardSet());
        if (PhotonNetwork.IsMasterClient)
        {
            professorDeck.ShuffleDeckAllClients();
            battleDeck.ShuffleDeckAllClients();
        }
    }

    public Card DrawProfessorCard(MonopolyPlayer drawingPlayer)
    {
        Card card = professorDeck.DrawCard();
        card.ApplyEffect(drawingPlayer, playerManager, board);
        return card;
    }

    public Card DrawTrainerBattleCard(MonopolyPlayer drawingPlayer)
    {
        Card card = battleDeck.DrawCard();
        card.ApplyEffect(drawingPlayer, playerManager, board);
        return card;
    }
}
