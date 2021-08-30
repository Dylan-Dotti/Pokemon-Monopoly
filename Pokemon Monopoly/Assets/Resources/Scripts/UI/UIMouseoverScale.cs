using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMouseoverScale : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector3 mouseoverScale = Vector3.one * 1.05f;
    [SerializeField] private float tweenTime = 0.15f;

    private Vector3 defaultScale;

    protected virtual void Awake()
    {
        defaultScale = transform.localScale;
    }

    protected virtual void OnDisable()
    {
        DefaultScale();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        MouseoverScale();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        DefaultScale();
    }

    protected void MouseoverScale()
    {
        LeanTween.cancel(gameObject);
        float progress = transform.localScale.InverseLerp(mouseoverScale, defaultScale);
        float tweenTimeAdjusted = tweenTime * progress;
        Debug.Log(tweenTimeAdjusted);
        transform.LeanScale(mouseoverScale, tweenTimeAdjusted);
    }

    protected void DefaultScale()
    {
        LeanTween.cancel(gameObject);
        float progress = transform.localScale.InverseLerp(defaultScale, mouseoverScale);
        float tweenTimeAdjusted = tweenTime * progress;
        Debug.Log(tweenTimeAdjusted);
        transform.LeanScale(defaultScale, tweenTimeAdjusted);
    }
}
