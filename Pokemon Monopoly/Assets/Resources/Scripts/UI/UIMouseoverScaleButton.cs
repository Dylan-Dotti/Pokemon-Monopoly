using System;
using System.Collections;
using UnityEngine.UI;

public class UIMouseoverScaleButton : UIMouseoverScale
{
    private Button button;

    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        StartCoroutine(MonitorInteractableCR());
    }

    private IEnumerator MonitorInteractableCR()
    {
        while (true)
        {
            enabled = button.interactable;
            yield return null;
        }
    }
}
