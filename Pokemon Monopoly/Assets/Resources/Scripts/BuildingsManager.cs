using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingsFactory))]
[RequireComponent(typeof(PhotonView))]
public class BuildingsManager : MonoBehaviour
{
    public static BuildingsManager Instance { get; private set; }

    private Queue<GameObject> marts;
    private Queue<GameObject> centers;

    private List<GameObject> lentMarts;
    private List<GameObject> lentCenters;

    private BuildingsFactory factory;
    private PhotonView pView;

    public int NumMartsRemaining => marts.Count;
    public int NumCentersRemaining => marts.Count;

    public bool CanGetMart => NumMartsRemaining > 0;
    public bool CanGetCenter => NumCentersRemaining > 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            marts = new Queue<GameObject>();
            centers = new Queue<GameObject>();
            lentMarts = new List<GameObject>();
            lentCenters = new List<GameObject>();
            pView = GetComponent<PhotonView>();
            factory = GetComponent<BuildingsFactory>();
            SpawnBuildings();
        }
        else throw new System.Exception("Can't have two instances of BuildingsManager");
    }

    public bool RequestMart(Vector3 scale, out GameObject mart)
    {
        mart = CanGetMart ? GetMart(scale) : null;
        return mart != null;
    }

    public bool RequestCenter(Vector3 scale, out GameObject center)
    {
        center = CanGetCenter ? GetCenter(scale) : null;
        return center != null;
    }

    public GameObject GetMart(Vector3 scale)
    {
        if (!CanGetMart) throw new System.Exception("No marts remaining");
        var building = GetBuilding(marts, scale);
        lentMarts.Add(building);
        return building;
    }

    public GameObject GetCenter(Vector3 scale)
    {
        if (!CanGetCenter) throw new System.Exception("No centers remaining");
        var building = GetBuilding(centers, scale);
        lentCenters.Add(building);
        return building;
    }

    public void ReturnBuilding(GameObject building)
    {
        if (lentMarts.Remove(building)) marts.Enqueue(building);
        else if (lentCenters.Remove(building)) centers.Enqueue(building);
        else throw new System.Exception("Building was not lent");
        building.transform.parent = transform;
        building.SetActive(false);
    }

    private GameObject GetBuilding(Queue<GameObject> buildingQueue, Vector3 scale)
    {
        var building = buildingQueue.Dequeue();
        building.SetActive(true);
        building.transform.parent = null;
        building.transform.localScale = scale;
        return building;
    }

    private void SpawnBuildings()
    {
        for (int i = 0; i < 32; i++)
        {
            var newMart = factory.CreateMart();
            marts.Enqueue(newMart);
            newMart.transform.parent = transform;
            newMart.SetActive(false);
        }
        for (int i = 0; i < 12; i++)
        {
            var newCenter = factory.CreateCenter();
            centers.Enqueue(newCenter);
            newCenter.transform.parent = transform;
            newCenter.SetActive(false);
        }
    }
}
