using System;
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
    [SerializeField] private RectTransform textContainer;
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
        inputPlaceholder = inputField.transform.Find("Placeholder").gameObject;
        textContainer.offsetMin = new Vector2(0, textContainer.offsetMin.y);
        textContainer.offsetMax = new Vector2(-30, textContainer.offsetMax.y);
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

    public void LogEventLocal(string message)
    {
        CreateAndAddText(message);
    }

    public void LogEventOthers(string message)
    {
        if (PhotonNetwork.IsConnected)
        {
            pView.RPC("RPC_LogEvent", RpcTarget.OthersBuffered, message);
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
            CreateAndAddText($"Player says: {message}");
        }
    }

    public void LogEventAllClients(string eventMessage)
    {
        if (PhotonNetwork.IsConnected)
        {
            pView.RPC("RPC_LogEvent", RpcTarget.AllBuffered, eventMessage);
        }
        else
        {
            LogEventLocal(eventMessage);
        }
    }

    public void ClearLog()
    {
        textItems.ForEach(t => Destroy(t.gameObject));
        textItems.Clear();
    }

    public void ClearLogAllClients()
    {
        if (PhotonNetwork.IsConnected) pView.RPC("RPC_ClearLog", RpcTarget.AllBuffered);
        else ClearLog();
    }

    private void CreateAndAddText(string message)
    {
        clampScrollbar = true;
        Text newMessage = Instantiate(textPrefab, textContainer);
        DateTime currTime = DateTime.Now;
        string hourString = currTime.Hour < 10 ?
            "0" + currTime.Hour.ToString() : currTime.Hour.ToString();
        string minString = currTime.Minute < 10 ?
            "0" + currTime.Minute.ToString() : currTime.Minute.ToString();
        newMessage.text = $"[{hourString}:{minString}] {message}";
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
        message = $"{playerName} says: {message}";
        CreateAndAddText(message);
    }

    [PunRPC]
    private void RPC_LogEvent(string message)
    {
        CreateAndAddText(message);
    }

    [PunRPC]
    private void RPC_ClearLog()
    {
        ClearLog();
    }
}
