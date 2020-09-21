using UnityEngine.Events;
using UnityEngine.EventSystems;

public interface IUIMouseEventEmitter<T> : 
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    event UnityAction<T> MouseEntered;
    event UnityAction<T> MouseExited;
    event UnityAction<T> MouseClicked;
}
