using UnityEngine;
using UnityEngine.UI;

public class LeaveJailButton : MonoBehaviour
{
    private MonopolyPlayer friendlyPlayer;
    private Button leaveJailButton;

    private void Awake()
    {
        MonopolyPlayer.Spawned += OnPlayerSpawned;
        leaveJailButton = GetComponentInChildren<Button>();
        leaveJailButton.onClick.AddListener(OnButtonPress);
    }

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        if (player.IsLocalPlayer)
        {
            friendlyPlayer = player;
            friendlyPlayer.Despawned += OnPlayerDespawned;
        }
    }

    private void OnPlayerDespawned(MonopolyPlayer player)
    {
        friendlyPlayer = null;
    }

    public void OnButtonPress()
    {
        if (friendlyPlayer != null)
        {
            friendlyPlayer.LeaveJailAllClients();
        }
        else
        {
            Debug.Log("Friendly player is null");
        }
    }
}
