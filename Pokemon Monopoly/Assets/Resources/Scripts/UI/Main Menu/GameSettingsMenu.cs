using UnityEngine;
using UnityEngine.UI;

public class GameSettingsMenu : MonoBehaviour
{
    [SerializeField] private InputField playerStartingMoney;
    [SerializeField] private CheckBox auctionUnboughtProperties;

    private GameConfig settings;

    private void Awake()
    {
        settings = GameConfig.Instance;
    }

    private void OnEnable()
    {
        auctionUnboughtProperties.Checked = settings.AuctionPropertyOnNoBuy;
        playerStartingMoney.text = settings.PlayerStartingMoney.ToString();
    }

    public void OnConfirm()
    {
        settings.AuctionPropertyOnNoBuy = auctionUnboughtProperties.Checked;
        settings.PlayerStartingMoney = int.Parse(playerStartingMoney.text);
    }
}
