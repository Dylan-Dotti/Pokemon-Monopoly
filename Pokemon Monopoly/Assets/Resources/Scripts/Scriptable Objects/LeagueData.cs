using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LeagueData", menuName = "Scriptable Objects/League Data")]
public class LeagueData : ScriptableObject
{
    [SerializeField] private string leagueName;
    [SerializeField] private List<PropertyCollection> collections;

    private List<PropertyCollection> instantiatedCollections;

    public string LeagueName => leagueName;
    public IReadOnlyList<PropertyCollection> PropertyCollections => 
        instantiatedCollections ?? collections;

    private void Awake()
    {
        instantiatedCollections = new List<PropertyCollection>(
            collections.Select(c => Instantiate(c)));
        instantiatedCollections.ForEach(c => c.League = this);
    }
}
