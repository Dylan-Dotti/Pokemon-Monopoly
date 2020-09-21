using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIMouseEventEmitter : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event UnityAction<UIMouseEventEmitter> MouseEntered;
    public event UnityAction<UIMouseEventEmitter> MouseExited;
    public event UnityAction<UIMouseEventEmitter> MouseClicked;

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEntered?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExited?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MouseClicked?.Invoke(this);
    }
}
