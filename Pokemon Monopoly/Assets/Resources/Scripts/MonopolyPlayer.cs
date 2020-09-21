using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class MonopolyPlayer : MonoBehaviour
{
    public static event UnityAction<MonopolyPlayer> Spawned;
    public event UnityAction<MonopolyPlayer> Despawned;

    [SerializeField] private PlayerAvatar avatarPrefab;

    private PhotonView pView;
    private MonopolyBoard board;
    private HashSet<PropertyData> properties;

    public string PlayerName { get; private set; } = "Unknown";
    public string AvatarImageName { get; private set; }
    public PlayerAvatar PlayerToken { get; set; }
    public int Money { get; set; } = 1500;
    public IReadOnlyCollection<PropertyData> Properties => properties;

    public bool IsLocalPlayer => pView.IsMine;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        board = GameObject.FindGameObjectWithTag("Board")
            .GetComponent<MonopolyBoard>();
        if (IsLocalPlayer)
        {
            pView.RPC("RPC_SpawnInit", RpcTarget.AllBuffered,
                PhotonNetwork.LocalPlayer.NickName,
                MultiplayerSettings.Instance.AvatarImageName);
        }
    }

    private void Start()
    {
        SpawnAvatar();
    }

    private void OnDestroy()
    {
        Despawned?.Invoke(this);
    }

    public void SpawnAvatar()
    {
        Debug.Log("Spawning avatar");
        PlayerToken = Instantiate(avatarPrefab);
        PlayerToken.Owner = this;
        PlayerToken.SpawnAtSquare(board.GetSpawnSquare());
    }

    public void MoveAvatarSequential(int numSquares, MoveDirection direction)
    {
        pView.RPC("RPC_MoveAvatar", RpcTarget.AllBuffered, numSquares);
    }

    public void PurchaseProperty(PropertyData property)
    {
        Debug.Log("Purchasing property: " + property.PropertyName);
        property.Owner = this;
        Money -= property.PurchaseCost;
    }

    private IEnumerator MoveAvatarCR(
        IReadOnlyList<BoardSquare> squareSequence, float interval)
    {
        for (int i = 0; i < squareSequence.Count - 1; i++)
        {
            PlayerToken.MoveToSquare(squareSequence[i]);
            yield return new WaitForSeconds(interval);
        }
        PlayerToken.MoveToSquare(
            squareSequence[squareSequence.Count - 1],
            isLastMove: true);
    }

    [PunRPC]
    private void RPC_SpawnInit(string name, string avatarImageName)
    {
        PlayerName = name;
        AvatarImageName = avatarImageName;
        Spawned?.Invoke(this);
    }

    [PunRPC]
    private void RPC_MoveAvatar(int numSquares)
    {
        IReadOnlyList<BoardSquare> squareSequence =
            board.GetNextSquares(PlayerToken, numSquares);
        StartCoroutine(MoveAvatarCR(squareSequence, 0.5f));
    }
}
