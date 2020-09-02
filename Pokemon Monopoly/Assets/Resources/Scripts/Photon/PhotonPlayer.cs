using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(PhotonView))]
public class PhotonPlayer : MonoBehaviour
{
    public static UnityAction<PhotonPlayer> PlayerSpawnEvent;
    public static UnityAction<PhotonPlayer> PlayerDespawnEvent;

    public bool IsLocalPlayer => pView.IsMine;

    private PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        PlayerSpawnEvent?.Invoke(this);
        if (pView.IsMine)
        {

        }
    }

    private void Start()
    {

    }

    private void OnDestroy()
    {
        PlayerDespawnEvent?.Invoke(this);
    }
}
