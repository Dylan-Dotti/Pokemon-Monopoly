using UnityEngine;

[CreateAssetMenu(fileName = "PropertyCollection", menuName = "Scriptable Objects/Property Collection")]
public class PropertyCollection : ScriptableObject
{
    public string CollectionName => collectionName;

    [SerializeField] private string collectionName;
}
