using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyMenuButton : Button
{
    private PropertyData linkedProperty;
    private Image buttonImage;

    public PropertyData LinkedProperty
    {
        get => linkedProperty;
        set
        {
            linkedProperty = value;
            OnPropertySet();
        }
    }

    protected override void Awake()
    {
        buttonImage = transform.Find("Property Sprite").GetComponent<Image>();
    }

    private void OnPropertySet()
    {
        buttonImage.sprite = LinkedProperty.PropertySprite;
    }
}
