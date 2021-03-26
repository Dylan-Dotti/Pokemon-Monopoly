using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class RoomMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private MultiplayerInitController initController;

    private PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        startButton.onClick.AddListener(OnStartClicked);
        cancelButton.onClick.AddListener(OnCancelClicked);
    }

    private void OnStartClicked()
    {
        pView.RPC("RPC_StartGame", RpcTarget.AllBufferedViaServer);
    }

    private void OnCancelClicked()
    {

    }

    [PunRPC]
    private void RPC_StartGame()
    {
        initController.StartCountdown();
    }
}
