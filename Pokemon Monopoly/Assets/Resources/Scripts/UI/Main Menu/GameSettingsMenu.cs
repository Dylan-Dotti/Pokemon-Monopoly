using UnityEngine;
using UnityEngine.UI;

public class GameSettingsMenu : MonoBehaviour
{
    [SerializeField] private InputField playerStartingMoney;
    [SerializeField] private InputField maxNumMarts;
    [SerializeField] private InputField maxNumCenters;
    [SerializeField] private CheckBox auctionUnboughtProperties;

    private GameConfig settings;

    private void Awake()
    {
        settings = GameConfig.Instance;
    }

    private void OnEnable()
    {
        auctionUnboughtProperties.Checked = settings.AuctionPropertyOnNoBuy;
        maxNumMarts.text = settings.MaxNumMarts.ToString();
        maxNumCenters.text = settings.MaxNumCenters.ToString();
        playerStartingMoney.text = settings.PlayerStartingMoney.ToString();
    }

    public void OnConfirm()
    {
        settings.AuctionPropertyOnNoBuy = auctionUnboughtProperties.Checked;
        settings.MaxNumMarts = int.Parse(maxNumMarts.text);
        settings.MaxNumCenters = int.Parse(maxNumCenters.text);
        settings.PlayerStartingMoney = int.Parse(playerStartingMoney.text);
    }
}
