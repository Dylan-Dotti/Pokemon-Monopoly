using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyMenuDetailPanel : MonoBehaviour, IPropertyDisplay
{
    [SerializeField] private PropertyDetailsDisplay propDetailsDisplay;
    [SerializeField] private GameObject defaultPanel;

    private EditableTextButton mortgageButton;
    private Button upgradeButton;
    private Button downgradeButton;

    private MonopolyPlayer localPlayer;

    private bool DefaultPanelEnabled
    {
        get => defaultPanel.gameObject.activeSelf;
        set => defaultPanel.gameObject.SetActive(value);
    }

    private void Awake()
    {
        Transform detailsPanel = transform.Find("Details Display");
        DefaultPanelEnabled = true;
        mortgageButton = detailsPanel.Find("Mortgage Button").GetComponent<EditableTextButton>();
        upgradeButton = detailsPanel.Find("Upgrade Button").GetComponent<Button>();
        downgradeButton = detailsPanel.Find("Downgrade Button").GetComponent<Button>();

    }

    private void Start()
    {
        PlayerMoneyText moneyText = transform.Find("Money Panel")
            .GetComponentInChildren<PlayerMoneyText>();
        localPlayer = PlayerManager.Instance.LocalPlayer;
        moneyText.LinkedPlayer = localPlayer;
    }

    public void EnableDisplay(GymPropertyData gymData)
    {
        EnableBaseDisplay(gymData);
        SetUpgradeComponentsActive(true);
        propDetailsDisplay.EnableDisplay(gymData);

        upgradeButton.interactable = localPlayer != null &&
            gymData.Owner == localPlayer && gymData.Upgradable && 
            localPlayer.Money >= gymData.UpgradeCost;
        if (upgradeButton.interactable)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(
                () => localPlayer.UpgradeProperty(gymData));
        }
        downgradeButton.interactable = localPlayer != null &&
            gymData.Owner == localPlayer && gymData.Downgradable;
        if (downgradeButton.interactable)
        {
            downgradeButton.onClick.RemoveAllListeners();
            downgradeButton.onClick.AddListener(
                () => localPlayer.DowngradeProperty(gymData));
        }
    }

    public void EnableDisplay(BallPropertyData ballData)
    {
        EnableBaseDisplay(ballData);
        SetUpgradeComponentsActive(false);
        propDetailsDisplay.EnableDisplay(ballData);
    }

    public void EnableDisplay(LegendaryPropertyData legendData)
    {
        EnableBaseDisplay(legendData);
        SetUpgradeComponentsActive(false);
        propDetailsDisplay.EnableDisplay(legendData);
    }

    private void EnableBaseDisplay(PropertyData property)
    {
        DefaultPanelEnabled = false;
        if (property.IsMortgaged)
        {
            mortgageButton.text = "Pay Mortgage";
            mortgageButton.interactable = property.Owner != null &&
                property.Owner == localPlayer && 
                property.Owner.Money >= property.UnmortgageCost;
            if (mortgageButton.interactable)
            {
                mortgageButton.onClick.RemoveAllListeners();
                mortgageButton.onClick.AddListener(
                    () => localPlayer.UnmortgageProperty(property));
            }
        }
        else
        {
            mortgageButton.text = "Mortgage";
            mortgageButton.interactable =
                property.Owner == localPlayer && property.Mortgageable;
            if (mortgageButton.interactable)
            {
                mortgageButton.onClick.RemoveAllListeners();
                mortgageButton.onClick.AddListener(
                    () => localPlayer.MortgageProperty(property));
            }
        }
    }

    private void SetUpgradeComponentsActive(bool active)
    {
        upgradeButton.gameObject.SetActive(active);
        downgradeButton.gameObject.SetActive(active);
    }
}
