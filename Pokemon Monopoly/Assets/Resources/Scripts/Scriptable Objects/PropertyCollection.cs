using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PropertyCollection", menuName = "Scriptable Objects/Property Collection")]
public class PropertyCollection : ScriptableObject
{
    [SerializeField] private string collectionName;
    [SerializeField] private Material collectionMaterial;
    [SerializeField] private Material collectionUIMaterial;
    [SerializeField] private List<PropertyData> properties;
    [SerializeField] private LeagueData league;

    private List<PropertyData> instantiatedProperties;

    public string CollectionName => collectionName;
    public Material CollectionMaterial => collectionMaterial;
    public Material CollectionUIMaterial => collectionUIMaterial;
    public IReadOnlyList<PropertyData> Properties =>
        instantiatedProperties ?? properties;
    public LeagueData League
    {
        get => league;
        set => league = value;
    }

    public bool PlayerHasMonopoly(MonopolyPlayer player) =>
        NumPropsInCollectionOwned(player) == Properties.Count;

    public int NumPropsInCollectionOwned(MonopolyPlayer player) =>
        player == null ? 0 : Properties.Where(prop => prop.Owner == player).Count();

    private void Awake()
    {
        instantiatedProperties = new List<PropertyData>(
            properties.Select(p => Instantiate(p)));
        instantiatedProperties.ForEach(p => p.CollectionData = this);
    }
}
