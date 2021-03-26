using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerDisplay : MonoBehaviour
{
    [SerializeField] private Text playerNameText;

    private string playerName;

    public string PlayerName
    {
        get => playerName;
        set
        {
            playerName = value;
            playerNameText.text = value;
        }
    }
}
