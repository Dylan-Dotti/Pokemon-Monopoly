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

    public string PlayerName { get; set; }
    public PlayerAvatar Avatar { get; set; }

    public bool IsLocalPlayer => pView.IsMine;

    private PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        Avatar = Instantiate(avatarPrefab);
    }

    private void Start()
    {
        Spawned?.Invoke(this);
    }

    private void OnDestroy()
    {
        Despawned?.Invoke(this);
    }
}
