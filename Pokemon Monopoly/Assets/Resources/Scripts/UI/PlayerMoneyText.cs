using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoneyText : Text
{
    private MonopolyPlayer player;

    protected override void Awake()
    {
        base.Awake();
        MonopolyPlayer.Spawned += OnPlayerSpawned;
        enabled = false;
    }

    private void OnPlayerSpawned(MonopolyPlayer player)
    {
        if (player.IsLocalPlayer)
        {
            this.player = player;
            enabled = true;
        }
    }

    private void Update()
    {
        text = SpecialCharacters.POKEMONEY_SYMBOL + 
            player.Money.ToString();
    }
}
