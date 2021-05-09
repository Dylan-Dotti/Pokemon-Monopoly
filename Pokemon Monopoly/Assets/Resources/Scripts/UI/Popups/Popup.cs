using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Popup : MonoBehaviour
{
    public event UnityAction<Popup> PopupClosed;

    public bool IsOpen { get; private set; }

    public virtual Coroutine Open()
    {
        gameObject.SetActive(true);
        IsOpen = true;
        return StartCoroutine(PopupOpenCR());
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        IsOpen = false;
        PopupClosed?.Invoke(this);
    }

    private IEnumerator PopupOpenCR()
    {
        while (IsOpen) yield return null;
    }
}
