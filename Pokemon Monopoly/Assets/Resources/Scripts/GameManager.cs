using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;

    private void Awake()
    {
        playerManager.PlayersReady += OnPlayersReady;
    }

    private void OnPlayersReady()
    {
        playerManager.SwitchNextActivePlayer();
    }
}
