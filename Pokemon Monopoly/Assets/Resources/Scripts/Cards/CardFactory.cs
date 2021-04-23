using System.Collections.Generic;
using UnityEngine;

public class CardFactory : MonoBehaviour
{
    [Header("Professor Card Sets")]
    [SerializeField] private Card[] professorOakSet;

    [Header("Trainer Battle Card Sets")]
    [SerializeField] private Card[] kantoTrainerBattleSet;

    private void Awake()
    {
        professorOakSet = Resources.LoadAll<Card>("Scriptable Objects/Cards/Professor");
        kantoTrainerBattleSet = Resources.LoadAll<Card>("Scriptable Objects/Cards/Trainer Battle");
    }

    public IReadOnlyList<Card> GetProfessorCardSet()
    {
        return new List<Card>(professorOakSet);
    }

    public IReadOnlyList<Card> GetTrainerBattleCardSet()
    {
        return new List<Card>(kantoTrainerBattleSet);
    }
}
