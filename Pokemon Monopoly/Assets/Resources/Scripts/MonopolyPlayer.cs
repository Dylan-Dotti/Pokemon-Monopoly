using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class MonopolyPlayer : MonoBehaviour
{
    public string PlayerName { get; set; }
    public bool IsLocalPlayer => pView.IsMine;

    private PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
    }
}
