using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public abstract class PlayerValueText : MonoBehaviour
{
    private MonopolyPlayer player;
    private Text text;

    public MonopolyPlayer LinkedPlayer
    {
        get => player;
        set
        {
            player = value;
            enabled = value != null;
        }
    }

    private void Awake()
    {
        MonopolyPlayer.Spawned += OnPlayerSpawned;
        text = GetComponent<Text>();
    }

    protected abstract string GetValueText(MonopolyPlayer player);

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
        if (LinkedPlayer != null)
        {
            text.text = GetValueText(player);
        }
    }
}
