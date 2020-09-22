using UnityEngine;
using UnityEngine.EventSystems;

public class UnselectableUIObject : MonoBehaviour
{
    private void Update()
    {
        EventSystem e = EventSystem.current;
        if (e.currentSelectedGameObject == gameObject)
        {
            e.SetSelectedGameObject(null);
        }
    }
}
