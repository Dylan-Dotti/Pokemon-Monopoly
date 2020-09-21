using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatarPanelManager : MonoBehaviour
{
    [SerializeField] private PlayerAvatarPanel panelPrefab;
    [SerializeField] private float localPanelScale = 0.7f;
    [SerializeField] private float remotePanelScale = 0.5f;

    private PlayerManager pManager;
    private PlayerAvatarPanel localPlayerPanel;
    private Dictionary<MonopolyPlayer, PlayerAvatarPanel> remotePlayerPanels;

    private void Awake()
    {
        pManager = GameObject.Find("Player Manager")
            .GetComponent<PlayerManager>();
        localPlayerPanel = transform.GetChild(0).GetComponent<PlayerAvatarPanel>();
        remotePlayerPanels = new Dictionary<MonopolyPlayer, PlayerAvatarPanel>();
        pManager.PlayersReady += OnPlayersReady;
    }

    public void SetLocalPlayerPanel(MonopolyPlayer player)
    {
        localPlayerPanel.LinkedPlayer = player;
    }

    public void AddRemotePlayerPanel(MonopolyPlayer player)
    {
        if (!remotePlayerPanels.ContainsKey(player))
        {
            player.Despawned += OnPlayerDespawned;
            PlayerAvatarPanel newPanel = Instantiate(panelPrefab, transform);
            newPanel.transform.localScale = new Vector3(remotePanelScale, remotePanelScale, 1);
            newPanel.LinkedPlayer = player;
            remotePlayerPanels.Add(player, newPanel);
        }
    }

    public void RemoveRemotePlayerPanel(MonopolyPlayer player)
    {
        if (remotePlayerPanels.ContainsKey(player))
        {
            player.Despawned -= OnPlayerDespawned;
            Destroy(remotePlayerPanels[player].gameObject);
            remotePlayerPanels.Remove(player);
        }
    }

    private void OnPlayersReady()
    {
        SetLocalPlayerPanel(pManager.LocalPlayer);
        foreach (MonopolyPlayer player in pManager.RemotePlayers)
        {
            AddRemotePlayerPanel(player);
        }
    }

    private void OnPlayerDespawned(MonopolyPlayer player)
    {
        RemoveRemotePlayerPanel(player);
    }
}
