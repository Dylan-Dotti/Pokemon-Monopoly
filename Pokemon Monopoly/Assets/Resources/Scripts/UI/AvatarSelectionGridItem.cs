using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class AvatarSelectionGridItem : MonoBehaviour, 
    IUIMouseEventEmitter<AvatarSelectionGridItem>
{
    public event UnityAction<AvatarSelectionGridItem> MouseEntered;
    public event UnityAction<AvatarSelectionGridItem> MouseExited;
    public event UnityAction<AvatarSelectionGridItem> MouseClicked;

    [SerializeField] private GameObject avatarPrefab;
    private GameObject internalPrefab;

    public GameObject AvatarImagePrefab => avatarPrefab;

    private void Awake()
    {
        internalPrefab = avatarPrefab;
    }

    private void Update()
    {
        if (avatarPrefab != internalPrefab && transform.childCount == 1)
        {
            if (Application.isEditor) DestroyImmediate(transform.GetChild(0).gameObject);
            else Destroy(transform.GetChild(0).gameObject);
        }
        if (transform.childCount == 0)
        {
            internalPrefab = avatarPrefab;
            GameObject avatar = Instantiate(avatarPrefab, transform);
            avatar.transform.localScale = new Vector3(0.8f, 0.8f, 1);
            avatar.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MouseClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEntered?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExited?.Invoke(this);
    }
}
