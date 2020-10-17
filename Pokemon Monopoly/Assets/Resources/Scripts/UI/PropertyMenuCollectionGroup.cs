using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PropertyMenuCollectionGroup : MonoBehaviour
{
    public event UnityAction<PropertyData> PropertySelected;

    [SerializeField] private List<PropertyMenuCollectionPanel> collectionPanels;
    [SerializeField] private PropertyManager propertyManager;

    private void Awake()
    {

    }

    private void Start()
    {
        LeagueData league = propertyManager.GetLeagueByName("Kanto League");
        collectionPanels = new List<PropertyMenuCollectionPanel>(
            GetComponentsInChildren<PropertyMenuCollectionPanel>());
        for (int i = 0; i < collectionPanels.Count; i++)
        {
            collectionPanels[i].LinkedCollection = league.PropertyCollections[i];
            collectionPanels[i].PropertySelected += OnPropertySelected;
        }
    }

    private void OnPropertySelected(PropertyData property)
    {
        PropertySelected?.Invoke(property);
    }
}
