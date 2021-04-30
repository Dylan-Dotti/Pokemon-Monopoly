using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyMenu : Popup
{
    [SerializeField] private PropertyMenuCollectionGroup collectionGroup;
    [SerializeField] private PropertyMenuDetailPanel propDetailPanel;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Button closeButton;

    private PropertyData selectedProperty;
    private MonopolyPlayer localPlayer;

    private void Awake()
    {
        LeagueData league = PropertyManager.Instance.GetLeagueByName("Kanto League");
        collectionGroup.PropertySelected += OnPropertySelected;
    }

    private void Update()
    {
        selectedProperty?.EnablePropertyDisplay(propDetailPanel);
        closeButton.interactable = localPlayer == null || localPlayer.Money >= 0;
    }

    public override Coroutine Open()
    {
        localPlayer = PlayerManager.Instance.LocalPlayer;
        cameraController.SetAllControlsEnabled(false);
        return base.Open();
    }

    public override void Close()
    {
        base.Close();
        cameraController.SetAllControlsEnabled(true);
    }

    public void OnPropertySelected(PropertyData property)
    {
        selectedProperty = property;
    }
}
