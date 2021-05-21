using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField] private Transform backgroundBlocker;

    private Queue<PopupOpenCommand> popupQueue;
    private Stack<PopupOpenCommand> displayStack;
    private PhotonView pView;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            pView = GetComponent<PhotonView>();
            popupQueue = new Queue<PopupOpenCommand>();
            displayStack = new Stack<PopupOpenCommand>();
        }
    }

    public void QueuePopup(Popup nextPopup,
        PopupOpenOptions openOptions, bool blockBackground = true)
    {
        Debug.Log("Queueing popup");
        var nextCommand = new PopupOpenCommand(nextPopup, blockBackground);
        switch (openOptions)
        {
            case PopupOpenOptions.Queue:
                //popupQueue.Enqueue(nextCommand);
                GameActionQueue.Instance.QueueCoroutineAction(
                    () => ExecuteOpenCommand(nextCommand));
                //OnPopupEnqueue();
                break;
            case PopupOpenOptions.Overlay:
                ExecuteOpenCommand(nextCommand);
                break;
        }
    }

    private Coroutine ExecuteOpenCommand(PopupOpenCommand command)
    {
        if (command.BlockBackground)
        {
            backgroundBlocker.gameObject.SetActive(true);
            DetachPopupsFromBlocker();
            MoveBackgroundBlockerToFront();
            command.PopupToOpen.transform.parent = backgroundBlocker;
        }
        else
        {
            command.PopupToOpen.transform.parent = transform;
        }
        displayStack.Push(command);
        command.PopupToOpen.transform.localPosition = Vector3.zero;
        command.PopupToOpen.PopupClosed += OnCurrentPopupClosed;
        return command.PopupToOpen.Open();
    }

    private void MoveBackgroundBlockerToFront()
    {
        backgroundBlocker.transform.parent = null;
        backgroundBlocker.transform.parent = transform;
    }

    private void DetachPopupsFromBlocker()
    {
        int blockerIndex = transform.IndexOf(backgroundBlocker);
        Queue<Transform> tempRemovedChildren = new Queue<Transform>();
        // detach children below the background blocker from manager
        for (int i = blockerIndex + 1; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            tempRemovedChildren.Enqueue(child);
            child.transform.parent = null;
        }
        // move children of background blocker below blocker
        foreach (Transform child in backgroundBlocker.GetChildren())
        {
            child.transform.parent = transform;
        }
        // move detached children below former children of blocker
        foreach (Transform child in tempRemovedChildren)
        {
            child.transform.parent = transform;
        }
    }

    private void OnPopupEnqueue()
    {
        if (popupQueue.Count == 1)
        {
            ExecuteOpenCommand(popupQueue.Dequeue());
        }
    }

    private void OnCurrentPopupClosed(Popup popup)
    {
        popup.PopupClosed -= OnCurrentPopupClosed;
        if (displayStack.Pop().BlockBackground)
        {
            DetachPopupsFromBlocker();
            backgroundBlocker.gameObject.SetActive(false);
        }
        if (displayStack.Count > 0)
        {
            PopupOpenCommand nextDisplay = displayStack.Peek();
            if (nextDisplay.BlockBackground)
            {
                backgroundBlocker.gameObject.SetActive(true);
                nextDisplay.PopupToOpen.transform.parent = backgroundBlocker;
            }
        }
        else if (popupQueue.Count > 0)
        {
            ExecuteOpenCommand(popupQueue.Peek());
        }
    }

    private struct PopupOpenCommand
    {
        public Popup PopupToOpen { get; private set; }
        public bool BlockBackground { get; private set; }

        public PopupOpenCommand(Popup popupToOpen, bool blockBackground)
        {
            PopupToOpen = popupToOpen;
            BlockBackground = blockBackground;
        }
    }
}

public enum PopupOpenOptions { Queue, Overlay }
