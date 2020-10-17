using UnityEngine;
using UnityEngine.UI;

public class RentWithUpgradesDisplay : MonoBehaviour
{
    private Text[] gymTextObjects;
    private Text[] ballTextObjects;
    private Text[] legendaryTextObjects;

    private void Awake()
    {
        gymTextObjects = transform.Find("Gym Display")
            .GetComponentsInChildren<Text>(true);
        ballTextObjects = transform.Find("Ball Display")
            .GetComponentsInChildren<Text>(true);
        legendaryTextObjects = transform.Find("Legendary Display")
            .GetComponentsInChildren<Text>(true);
    }

    public void UpdateDisplay(GymPropertyData property)
    {
        gymTextObjects.ForEach(t => t.gameObject.SetActive(true));
        ballTextObjects.ForEach(t => t.gameObject.SetActive(false));
        legendaryTextObjects.ForEach(t => t.gameObject.SetActive(false));
        gymTextObjects[0].text = "Monopoly --- " + property.RentWithMonopoly.ToPokeMoneyString();
        gymTextObjects[1].text = "Mart x1 --- " + property.RentWithMarts(1).ToPokeMoneyString();
        gymTextObjects[2].text = "Mart x2 --- " + property.RentWithMarts(2).ToPokeMoneyString();
        gymTextObjects[3].text = "Mart x3 --- " + property.RentWithMarts(3).ToPokeMoneyString();
        gymTextObjects[4].text = "Mart x4 --- " + property.RentWithMarts(4).ToPokeMoneyString();
        gymTextObjects[5].text = "Center --- " + property.RentWithCenter.ToPokeMoneyString();
    }

    public void UpdateDisplay(BallPropertyData property)
    {
        gymTextObjects.ForEach(t => t.gameObject.SetActive(false));
        ballTextObjects.ForEach(t => t.gameObject.SetActive(true));
        legendaryTextObjects.ForEach(t => t.gameObject.SetActive(false));
    }

    public void UpdateDisplay(LegendaryPropertyData property)
    {
        gymTextObjects.ForEach(t => t.gameObject.SetActive(false));
        ballTextObjects.ForEach(t => t.gameObject.SetActive(false));
        legendaryTextObjects.ForEach(t => t.gameObject.SetActive(true));
    }
}
