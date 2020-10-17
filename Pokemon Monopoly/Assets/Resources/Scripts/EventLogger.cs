using UnityEngine;

public class EventLogger : MonoBehaviour
{
    public static EventLogger Instance { get; private set; }

    [SerializeField] private MultiplayerMessageLog eventMessageLog;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void LogEventLocal(string eventMessage)
    {
        eventMessageLog.LogEventLocal(eventMessage);
    }

    public void LogEventOtherClients(string eventMessage)
    {
        eventMessageLog.LogEventOthers(eventMessage);
    }

    public void LogEventAllClients(string eventMessage)
    {
        eventMessageLog.LogEventAllClients(eventMessage);
    }
}
