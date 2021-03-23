using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private PlayerUIManager uiManager;

    private PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        playerManager.PlayersReady += OnPlayersReady;
    }

    private void OnPlayersReady()
    {
        playerManager.SwitchNextActivePlayer();
    }

    // called only on local client
    public void EndTurn()
    {
        StartNextTurnAllClients();
    }

    private void StartNextTurnAllClients()
    {
        pView.RPC("RPC_StartNextTurn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPC_StartNextTurn()
    {
        playerManager.SwitchNextActivePlayer();
    }
}
