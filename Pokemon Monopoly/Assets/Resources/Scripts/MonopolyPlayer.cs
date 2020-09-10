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

    public string PlayerName { get; set; } = "Unknown";
    public PlayerAvatar Avatar { get; set; }

    public bool IsLocalPlayer => pView.IsMine;

    private PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        Avatar = Instantiate(avatarPrefab);
        if (IsLocalPlayer)
        {
            pView.RPC("RPC_SpawnInit", RpcTarget.AllBuffered,
                PhotonNetwork.LocalPlayer.NickName);
        }
    }

    private void OnDestroy()
    {
        Despawned?.Invoke(this);
    }

    public void MoveAvatarTo(BoardSquare square)
    {
        Avatar.MoveToSquare(square);
    }

    public Coroutine MoveAvatarSequential(
        IReadOnlyList<BoardSquare> squareSequence)
    {
        return StartCoroutine(MoveAvatarCR(squareSequence, 0.5f));
    }

    private IEnumerator MoveAvatarCR(
    IReadOnlyList<BoardSquare> squareSequence, float interval)
    {
        for (int i = 0; i < squareSequence.Count - 1; i++)
        {
            Avatar.MoveToSquare(squareSequence[i]);
            yield return new WaitForSeconds(interval);
        }
        Avatar.MoveToSquare(
            squareSequence[squareSequence.Count - 1]);
    }

    [PunRPC]
    private void RPC_SpawnInit(string name)
    {
        PlayerName = name;
        Spawned?.Invoke(this);
    }
}
