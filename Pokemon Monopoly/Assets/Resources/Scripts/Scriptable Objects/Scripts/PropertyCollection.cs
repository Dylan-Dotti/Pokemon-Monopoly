using UnityEngine;

[CreateAssetMenu(fileName = "PropertyCollection", menuName = "Scriptable Objects/Property Collection")]
public class PropertyCollection : ScriptableObject
{
    public string CollectionName => collectionName;
    public Material CollectionMaterial => collectionMaterial;

    [SerializeField] private string collectionName;
    [SerializeField] private Material collectionMaterial;
}
