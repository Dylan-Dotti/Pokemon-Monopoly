using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance { get; private set; }

    [SerializeField] private Button viewPropertiesButton;
    [SerializeField] private Button rollButton;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private MultiplayerMessageLog mainMessageLog;

    public bool ViewPropertiesInteractable
    {
        get => viewPropertiesButton.interactable;
        set => viewPropertiesButton.interactable = value;
    }

    public bool RollButtonInteractable
    {
        get => rollButton.interactable;
        set => rollButton.interactable = value;
    }

    public bool EndTurnInteractable
    {
        get => endTurnButton.interactable;
        set => endTurnButton.interactable = value;
    }

    public bool MessageLogInteractable
    {
        get => mainMessageLog.InputEnabled;
        set => mainMessageLog.InputEnabled = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
