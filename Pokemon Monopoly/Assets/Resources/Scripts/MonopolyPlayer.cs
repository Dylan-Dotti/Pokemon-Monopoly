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

    public string PlayerName { get; set; } = "Unknown";
    public PlayerAvatar PlayerToken { get; set; }
    public int Money { get; set; } = 1500;

    public IReadOnlyCollection<GymPropertyData> GymProperties { get; private set; }
    public IReadOnlyCollection<BallPropertyData> BallProperties { get; private set; }
    public IReadOnlyCollection<LegendaryPropertyData> LegendaryProperties { get; private set; }

    public bool IsLocalPlayer => pView.IsMine;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        board = GameObject.FindGameObjectWithTag("Board")
            .GetComponent<MonopolyBoard>();
        if (IsLocalPlayer)
        {
            pView.RPC("RPC_SpawnInit", RpcTarget.AllBuffered,
                PhotonNetwork.LocalPlayer.NickName);
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

    public void MoveAvatarSequential(int numSquares)
    {
        pView.RPC("RPC_MoveAvatar", RpcTarget.AllBuffered, numSquares);
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
    private void RPC_SpawnInit(string name)
    {
        PlayerName = name;
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
