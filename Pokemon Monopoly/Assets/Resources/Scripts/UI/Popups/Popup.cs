using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Popup : MonoBehaviour
{
    public event UnityAction<Popup> PopupClosed;

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        PopupClosed?.Invoke(this);
    }
}
