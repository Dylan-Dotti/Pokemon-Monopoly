using UnityEngine;

[CreateAssetMenu(fileName = "GymData", menuName = "Scriptable Objects/Gym Data")]
public sealed class GymData : ScriptableObject
{
    public string GymName => gymName;
    public Material GymColorMaterial => gymColorMaterial;
    public Color GymColor => gymColorMaterial.color;

    [SerializeField] private string gymName;
    [SerializeField] private Material gymColorMaterial;
}
