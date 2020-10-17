using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Deck : MonoBehaviour
{
    private PhotonView pView;
    private List<Card> cards;

    private GameObject deckObject;

    public IReadOnlyList<Card> Cards => cards;
    public int NumCardsRemaining => cards.Count;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        cards = new List<Card>(16);
        deckObject = transform.GetChild(0).gameObject;
    }

    public void SpawnDeck(IEnumerable<Card> cards)
    {
        this.cards = new List<Card>(cards);
        Debug.Log($"Spawning deck with {cards.Count()} cards");
        deckObject.SetActive(true);
    }

    public Card DrawCard()
    {
        if (NumCardsRemaining == 0) return null;
        Card topCard = cards[0];
        cards.RemoveAt(0);
        return topCard;
    }

    public void ShuffleDeckAllClients()
    {
        var indexesRandomized = 
            Enumerable.Range(0, cards.Count).Randomized().ToArray();
        pView.RPC("RPC_ShuffleDeck", RpcTarget.AllBufferedViaServer, indexesRandomized);
    }

    [PunRPC]
    private void RPC_ShuffleDeck(int[] cardIndexSequence)
    {
        cards = cardIndexSequence.Select(i => cards[i]).ToList();
        foreach (var card in Cards)
        {
            Debug.Log(card.Description);
        }
    }
}
