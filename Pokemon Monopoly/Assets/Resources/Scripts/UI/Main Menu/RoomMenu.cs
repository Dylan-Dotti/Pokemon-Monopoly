using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class RoomMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button startingButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button cancelingButton;
    [SerializeField] private Button leaveRoomButton;
    [SerializeField] private GameInitializer initializer;

    private PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        startButton.onClick.AddListener(OnStartClicked);
        cancelButton.onClick.AddListener(OnCancelClicked);
        leaveRoomButton.onClick.AddListener(OnLeaveRoomClicked);
    }

    private void OnStartClicked()
    {
        startButton.gameObject.SetActive(false);
        startingButton.gameObject.SetActive(true);
        pView.RPC("RPC_StartGame", RpcTarget.AllBufferedViaServer);
    }

    private void OnCancelClicked()
    {
        cancelButton.gameObject.SetActive(false);
        cancelingButton.gameObject.SetActive(true);
        pView.RPC("RPC_CancelStartGame", RpcTarget.AllBufferedViaServer);
    }

    private void OnLeaveRoomClicked()
    {
        PhotonNetwork.LeaveRoom();
        initializer.StopCountdown();
    }

    [PunRPC]
    private void RPC_StartGame()
    {
        if (!initializer.IsGameStarting) initializer.StartCountdown();
        startButton.gameObject.SetActive(false);
        startingButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(true);
    }

    [PunRPC]
    private void RPC_CancelStartGame()
    {
        if (initializer.IsGameStarting) initializer.StopCountdown();
        cancelButton.gameObject.SetActive(false);
        cancelingButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
    }
}
