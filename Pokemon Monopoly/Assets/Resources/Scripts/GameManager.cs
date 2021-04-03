using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;

    private PhotonView pView;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
        playerManager.PlayersReady += OnPlayersReady;
    }

    // called on all clients
    private void OnPlayersReady()
    {
        StartNextTurnLocal();
    }

    // called only on local client
    public void EndTurnAllClients()
    {
        StartNextTurnAllClients();
    }

    private void StartNextTurnLocal()
    {
        playerManager.SwitchNextActivePlayerLocal();
    }

    private void StartNextTurnAllClients()
    {
        if (pView == null) Debug.Log("pview null");
        pView.RPC("RPC_StartNextTurn", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void RPC_StartNextTurn()
    {
        StartNextTurnLocal();
    }
}
