using UnityEngine;

public class BuildingsFactory : MonoBehaviour
{
    [SerializeField] private GameObject martPrefab;
    [SerializeField] private GameObject centerPrefab;

    public GameObject CreateMart()
    {
        return Instantiate(martPrefab);
    }

    public GameObject CreateCenter()
    {
        return Instantiate(centerPrefab);
    }
}
