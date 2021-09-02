using UnityEngine;
using UnityEngine.EventSystems;

public class CheckBox : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject textObject;

    public bool Checked
    {
        get => textObject.activeSelf;
        set => textObject.SetActive(value);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        textObject.SetActive(!textObject.activeSelf);
    }
}
