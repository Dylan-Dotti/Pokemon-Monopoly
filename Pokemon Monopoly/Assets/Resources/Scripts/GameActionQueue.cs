using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameActionQueue : MonoBehaviour
{
    public static GameActionQueue Instance { get; private set; }

    public event UnityAction CompletedAction;
    public event UnityAction CompletedAllActions;

    // signals whether the next action
    // should be taken from sync or coroutine queue
    private Queue<bool> isSyncQueue;
    private Queue<UnityAction> syncActionQueue;
    private Queue<Func<Coroutine>> coroutineActionQueue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            isSyncQueue = new Queue<bool>();
            syncActionQueue = new Queue<UnityAction>();
            coroutineActionQueue = new Queue<Func<Coroutine>>();
        }
    }

    public void QueueAction(UnityAction action)
    {
        syncActionQueue.Enqueue(action);
        isSyncQueue.Enqueue(true);
        OnActionQueued();
    }

    public void QueueCoroutineAction(Func<Coroutine> action)
    {
        coroutineActionQueue.Enqueue(action);
        isSyncQueue.Enqueue(false);
        OnActionQueued();
    }

    private void ExecuteSynchronousAction(UnityAction action)
    {
        action();
        OnActionCompleted(true);
    }

    private void ExecuteCoroutineAction(Func<Coroutine> actionFunc)
    {
        StartCoroutine(ExecuteCoroutineActionCR(actionFunc()));
    }

    private IEnumerator ExecuteCoroutineActionCR(Coroutine coroutine)
    {
        yield return coroutine;
        OnActionCompleted(false);
    }

    private void ExecuteNextAction()
    {
        if (isSyncQueue.Peek())
        {
            ExecuteSynchronousAction(syncActionQueue.Peek());
        }
        else
        {
            ExecuteCoroutineAction(coroutineActionQueue.Peek());
        }
    }

    private void OnActionQueued()
    {
        if (isSyncQueue.Count == 1) ExecuteNextAction();
    }

    private void OnActionCompleted(bool isSyncAction)
    {
        CompletedAction?.Invoke();
        isSyncQueue.Dequeue();
        if (isSyncAction) syncActionQueue.Dequeue();
        else coroutineActionQueue.Dequeue();

        if (isSyncQueue.Count > 0) ExecuteNextAction();
        else CompletedAllActions?.Invoke();
    }
}
