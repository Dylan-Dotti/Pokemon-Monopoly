using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//[ExecuteAlways]
public class PropertyMenuCollectionPanel : MonoBehaviour
{
    public event UnityAction<PropertyData> PropertySelected;

    private PropertyCollection linkedCollection;
    private Text collectionNameText;
    private List<PropertyMenuButton> propertyButtons;

    public PropertyCollection LinkedCollection
    {
        get => linkedCollection;
        set
        {
            linkedCollection = value;
            OnCollectionSet();
        }
    }

    private void Awake()
    {
        collectionNameText = transform.Find("Collection Name Panel")
            .GetComponentInChildren<Text>();
        Transform buttonGroup = transform.Find("Property Buttons");
        propertyButtons = new List<PropertyMenuButton>(
            buttonGroup.GetComponentsInChildren<PropertyMenuButton>());
    }

    private void OnCollectionSet()
    {
        Debug.Log("New collection: " + LinkedCollection.CollectionName);
        collectionNameText.text = LinkedCollection.CollectionName;
        IReadOnlyList<PropertyData> properties = LinkedCollection.Properties;
        propertyButtons[2].gameObject.SetActive(properties.Count >= 3);
        propertyButtons[3].gameObject.SetActive(properties.Count >= 4);
        for (int i = 0; i < properties.Count; i++)
        {
            propertyButtons[i].LinkedProperty = properties[i];
        }
        foreach (PropertyMenuButton button in propertyButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(
                () => OnButtonClicked(button));
        }
    }

    private void OnButtonClicked(PropertyMenuButton button)
    {
        PropertySelected?.Invoke(button.LinkedProperty);
    }
}
