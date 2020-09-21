using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropertyManager : MonoBehaviour
{
    public static PropertyManager Instance { get; private set; }

    private List<PropertyData> properties;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            properties = new List<PropertyData>();
            properties.AddRange(Resources.LoadAll<PropertyData>(
                "Scripts/Scriptable Objects/Properties"));
        }
    }

    public PropertyData GetPropertyByName(string name)
    {
        return properties.Where(p => p.PropertyName == name)
            .FirstOrDefault();
    }
}
