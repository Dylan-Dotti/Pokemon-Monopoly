using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionQueue
{
    public event UnityAction CompletedAllActions;

    private readonly MonoBehaviour coroutineHost;

    // signals whether the next action
    // should be taken from sync or coroutine queue
    private readonly Queue<bool> isSyncQueue;
    private readonly Queue<UnityAction> syncActionQueue;
    private readonly Queue<Func<Coroutine>> coroutineActionQueue;

    public ActionQueue(MonoBehaviour coroutineHost)
    {
        this.coroutineHost = coroutineHost;
        isSyncQueue = new Queue<bool>();
        syncActionQueue = new Queue<UnityAction>();
        coroutineActionQueue = new Queue<Func<Coroutine>>();
    }

    public void QueueSynchronousAction(UnityAction action)
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
        coroutineHost.StartCoroutine(ExecuteCoroutineActionCR(actionFunc()));
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
        isSyncQueue.Dequeue();
        if (isSyncAction) syncActionQueue.Dequeue();
        else coroutineActionQueue.Dequeue();

        if (isSyncQueue.Count > 0) ExecuteNextAction();
        else CompletedAllActions?.Invoke();
    }
}
