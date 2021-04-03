using UnityEngine.Events;

public class LocalPlayerSpawnMonitor
{
    public event UnityAction<MonopolyPlayer> LocalPlayerChanged;

    private MonopolyPlayer localPlayer;

    public MonopolyPlayer LocalPlayer
    {
        get => localPlayer;
        set
        {
            localPlayer = value;
            LocalPlayerChanged?.Invoke(localPlayer);
        }
    }

    public LocalPlayerSpawnMonitor()
    {
        MonopolyPlayer.Spawned += OnPlayerSpawned;
    }

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        if (player.IsLocalPlayer && player != localPlayer)
        {
            player.Despawned += OnPlayerDespawned;
            LocalPlayer = player;
        }
    }

    private void OnPlayerDespawned(MonopolyPlayer player)
    {
        if (localPlayer != null && localPlayer == player)
        {
            LocalPlayer = null;
        }
    }
}
