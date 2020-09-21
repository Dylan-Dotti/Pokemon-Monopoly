using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    [SerializeField] private string avatarImageName;
    [SerializeField] private int menuScene;
    [SerializeField] private int multiplayerScene;

    public static MultiplayerSettings Instance { get; private set; }

    public string PlayerUsername { get; set; }
    public string AvatarImageName
    {
        get => avatarImageName;
        set => avatarImageName = value;
    }

    public int MenuScene { get => menuScene; set => menuScene = value; }
    public int MultiplayerScene { get => multiplayerScene; set => multiplayerScene = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
