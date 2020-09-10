using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhotonView))]
public class DiceRoller : MonoBehaviour
{
    public event UnityAction<DiceRoller> RollComplete;

    [SerializeField] private DiceRollPanel dicePanel;

    private PhotonView pView;

    public (int roll1, int roll2) LastRoll { get; private set; }
    public int LastRollTotal => LastRoll.roll1 + LastRoll.roll2;

    private void Awake()
    {
        pView = GetComponent<PhotonView>();
    }

    public void RollDice(string playerName)
    {
        int roll1 = Random.Range(1, 7);
        int roll2 = Random.Range(1, 7);
        LastRoll = (roll1, roll2);
        if (PhotonNetwork.IsConnectedAndReady)
        {
            pView.RPC("RPC_RollDice", RpcTarget.AllBuffered,
                playerName, roll1, roll2);
        }
        else
        {
            StartCoroutine(RollDiceCR(playerName, roll1, roll2));
        }
    }

    private IEnumerator RollDiceCR(
        string playerName, int roll1, int roll2)
    {
        dicePanel.ActivateAndStartRoll(playerName);
        yield return new WaitForSeconds(2);
        dicePanel.StopRolling(roll1, roll2);
        yield return new WaitForSeconds(2);
        dicePanel.Deactivate();
        RollComplete?.Invoke(this);
    }

    [PunRPC]
    private void RPC_RollDice(
        string playerName, int roll1, int roll2)
    {
        StartCoroutine(RollDiceCR(playerName, roll1, roll2));
    }
}
