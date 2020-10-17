using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyMenu : Popup
{
    [SerializeField] private PropertyMenuCollectionGroup collectionGroup;
    [SerializeField] private PropertyMenuDetailPanel propDetailPanel;
    [SerializeField] private CameraController cameraController;

    private PropertyData selectedProperty;

    private void Awake()
    {
        LeagueData league = PropertyManager.Instance.GetLeagueByName("Kanto League");
        collectionGroup.PropertySelected += OnPropertySelected;
    }

    private void Update()
    {
        selectedProperty?.EnablePropertyDisplay(propDetailPanel);
    }

    public override void Open()
    {
        base.Open();
        cameraController.SetAllControlsEnabled(false);
    }

    public override void Close()
    {
        base.Close();
        cameraController.SetAllControlsEnabled(true);
    }

    public void OnPropertySelected(PropertyData property)
    {
        Debug.Log("Property selected: " + property.PropertyName);
        selectedProperty = property;
    }
}
