using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerInitController : MonoBehaviour
{
    [SerializeField] private PhotonRoom room;

    [SerializeField] private int startingTime = 3;
    [SerializeField] private Text waitingText;
    [SerializeField] private Text countdownText;

    private CountdownTimer timer;

    private void Awake()
    {
        timer = GetComponent<CountdownTimer>();
        timer.SecondsRemainingChanged += OnCountdownSecondsChanged;
        timer.CountdownCompleted += OnCountdownCompleted;
        room.LocalPlayerJoined += OnLocalPlayerJoined;
        room.LocalPlayerLeft += OnLocalPlayerLeft;
        room.ReachedMinPlayers += OnReachedMinPlayers;
        room.BelowMinPlayers += OnBelowMinPlayers;
    }

    private void OnDestroy()
    {
        room.LocalPlayerJoined -= OnLocalPlayerJoined;
        room.LocalPlayerLeft -= OnLocalPlayerLeft;
        room.ReachedMinPlayers -= OnReachedMinPlayers;
        room.BelowMinPlayers -= OnBelowMinPlayers;
    }

    public void StartCountdown()
    {
        SetWaitingAndCountdownActive(false, true);
        timer.StartCountdown(startingTime);
    }

    public void StopCountdown()
    {
        SetWaitingAndCountdownActive(true, false);
        timer.StopCountdown();
    }

    public void StartGame()
    {
        Debug.Log("Starting game");
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(MultiplayerSettings.Instance.MultiplayerScene);
    }

    private void OnLocalPlayerJoined()
    {
        SetWaitingAndCountdownActive(true, false);
    }

    private void OnLocalPlayerLeft()
    {
        SetWaitingAndCountdownActive(false, false);
    }

    private void OnReachedMinPlayers()
    {
        StartCountdown();
    }

    private void OnBelowMinPlayers()
    {
        StopCountdown();
    }

    private void OnCountdownSecondsChanged(float secondsRemaining)
    {
        countdownText.text = "Starting game in: " +
            Mathf.CeilToInt(secondsRemaining);
    }

    private void OnCountdownCompleted()
    {
        Debug.Log("Countdown complete");
        StartGame();
    }

    private void SetWaitingAndCountdownActive(bool waitingActive, bool countingActive)
    {
        waitingText.gameObject.SetActive(waitingActive);
        countdownText.gameObject.SetActive(countingActive);
    }
}
