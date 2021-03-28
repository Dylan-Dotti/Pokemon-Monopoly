using UnityEngine;
using UnityEngine.UI;

public class PlayerControlPanel : MonoBehaviour
{
    public static PlayerControlPanel Instance { get; private set; }

    [SerializeField] private Button viewPropertiesButton;
    [SerializeField] private Button rollAndMoveButton;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private Button leaveJailButton;

    public bool ViewPropertiesEnabled
    {
        set => viewPropertiesButton.interactable = value;
    }

    public bool RollAndMoveEnabled
    {
        set => rollAndMoveButton.interactable = value;
    }

    public bool EndTurnEnabled
    {
        set => endTurnButton.interactable = value;
    }

    public bool LeaveJailEnabled
    {
        set => leaveJailButton.interactable = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        ViewPropertiesEnabled = true;
        RollAndMoveEnabled = true;
        EndTurnEnabled = true;
        LeaveJailEnabled = true;
    }

    private void OnDisable()
    {
        ViewPropertiesEnabled = false;
        RollAndMoveEnabled = false;
        EndTurnEnabled = false;
        LeaveJailEnabled = false;
    }
}
