﻿using System;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class MultiplayerMessageLog : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Scrollbar vScrollbar;

    [SerializeField] private PlayerManager pManager;
    [SerializeField] private Transform textContainer;
    [SerializeField] private Text textPrefab;

    private bool enterEnabled;
    private bool clampScrollbar;
    private List<Text> textItems;
    private PhotonView pView;

    private GameObject inputPlaceholder;

    private void Awake()
    {
        textItems = new List<Text>();
        pView = GetComponent<PhotonView>();
        inputPlaceholder = inputField.transform
            .Find("Placeholder").gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (enterEnabled)
            {
                if (IsInputValid(inputField.text))
                {
                    LogMessageAllClients(inputField.text);
                    inputField.text = "";
                }
                EventSystem.current.SetSelectedGameObject(null);
            }
            else FocusInputField();
        }
        enterEnabled = inputField.isFocused;
        inputPlaceholder.SetActive(!inputField.isFocused);
        // clamp scroll bar to bottom
        if (clampScrollbar && vScrollbar.value != 0)
        {
            vScrollbar.value = 0;
            clampScrollbar = false;
        }
    }

    public void LogMessageAllClients(string message)
    {
        if (PhotonNetwork.IsConnected)
        {
            pView.RPC("RPC_LogMessage", RpcTarget.AllBuffered,
                pManager.LocalPlayer.PlayerName, message);
        }
        else
        {
            CreateAndAddText("Player", message);
        }
    }

    private void CreateAndAddText(string playerName, string message)
    {
        clampScrollbar = true;
        Text newMessage = Instantiate(textPrefab, textContainer);
        DateTime currTime = DateTime.Now;
        string hourString = currTime.Hour < 10 ?
            "0" + currTime.Hour.ToString() : currTime.Hour.ToString();
        string minString = currTime.Minute < 10 ?
            "0" + currTime.Minute.ToString() : currTime.Minute.ToString();
        newMessage.text = $"[{hourString}:{minString}] {playerName} says: {message}";
        textItems.Add(newMessage);
    }

    private void FocusInputField()
    {
        EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
        inputField.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    private bool IsInputValid(string input)
    {
        return input.Length > 0;
    }

    [PunRPC]
    private void RPC_LogMessage(string playerName, string message)
    {
        CreateAndAddText(playerName, message);
    }
}