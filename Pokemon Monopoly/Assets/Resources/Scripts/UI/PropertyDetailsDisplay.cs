using UnityEngine;
using UnityEngine.UI;

public class PropertyDetailsDisplay : MonoBehaviour, IPropertyDisplay
{
    [SerializeField] private Image propertyImage;
    [SerializeField] private Text propertyNameText;
    [SerializeField] private Text propertyOwnerText;
    [SerializeField] private Text purchaseCostText;
    [SerializeField] private Text currentRentText;
    [SerializeField] private Text baseRentText;
    [SerializeField] private Text upgradesText;
    [SerializeField] private Text mortgageValueText;
    [SerializeField] private Text upgradeCostText;
    [SerializeField] private Text downgradeReturnText;
    [SerializeField] private RentWithUpgradesDisplay upgradesRentDisplay;

    public void EnableDisplay(GymPropertyData property)
    {
        EnableBaseDisplay(property);
        SetUpgradeComponentsActive(true);
        DisplayUpgradeCost(property);
        DisplayDowngradeReturn(property);
        upgradesRentDisplay.UpdateDisplay(property);
    }

    public void EnableDisplay(BallPropertyData property)
    {
        EnableBaseDisplay(property);
        SetUpgradeComponentsActive(false);
        upgradesRentDisplay.UpdateDisplay(property);
    }

    public void EnableDisplay(LegendaryPropertyData property)
    {
        EnableBaseDisplay(property);
        SetUpgradeComponentsActive(false);
        DisplayBaseRent(property);
        DisplayCurrentRent(property);
        upgradesRentDisplay.UpdateDisplay(property);
    }

    private void EnableBaseDisplay(PropertyData property)
    {
        DisplayPropertyName(property);
        DisplayPropertySprite(property);
        DisplayPropertyOwner(property);
        DisplayBaseRent(property);
        DisplayCurrentRent(property);
        DisplayPurchaseCost(property);
        DisplayMortgageValue(property);
    }

    private void DisplayPropertyName(PropertyData property)
    {
        propertyNameText.text = property.PropertyName;
    }

    private void DisplayPropertySprite(PropertyData property)
    {
        propertyImage.sprite = property.PropertySprite;
    }

    private void DisplayPropertyOwner(PropertyData property)
    {
        MonopolyPlayer owner = property.Owner;
        propertyOwnerText.text = "Owner: " +
            (owner == null ? "None" : owner.PlayerName);
    }

    private void DisplayBaseRent(LegendaryPropertyData property)
    {
        baseRentText.text = "Base rent: " +
            property.BaseRent.ToPokeMoneyString() + " x last roll";
    }

    private void DisplayBaseRent(PropertyData property)
    {
        baseRentText.text = "Base rent: " +
            property.BaseRent.ToPokeMoneyString();
    }

    private void DisplayCurrentRent(LegendaryPropertyData property)
    {
        currentRentText.text = "Current rent: " +
            property.CurrentRentWithoutRoll.ToPokeMoneyString() + " x last roll";
    }

    private void DisplayCurrentRent(PropertyData property)
    {
        currentRentText.text = "Current rent: " +
            property.CurrentRent.ToPokeMoneyString();
    }

    private void DisplayPurchaseCost(PropertyData property)
    {
        purchaseCostText.text = "Purchase cost: " +
            property.PurchaseCost.ToPokeMoneyString();
    }

    private void DisplayMortgageValue(PropertyData property)
    {
        mortgageValueText.text = "Mortgage value: " +
            property.MortgageValue.ToPokeMoneyString();
    }

    private void DisplayUpgradeCost(GymPropertyData property)
    {
        upgradeCostText.text = "Upgrade cost: " +
            property.UpgradeCost.ToPokeMoneyString();
    }

    private void DisplayDowngradeReturn(GymPropertyData property)
    {
        downgradeReturnText.text = "Downgrade return: " +
            property.DowngradeValue.ToPokeMoneyString();
    }

    private void SetUpgradeComponentsActive(bool active)
    {
        upgradeCostText.gameObject.SetActive(active);
        downgradeReturnText.gameObject.SetActive(active);
    }
}
