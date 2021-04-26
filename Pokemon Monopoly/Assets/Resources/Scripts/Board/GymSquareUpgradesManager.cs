using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GymSquareUpgradesManager : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float spacingBetweenPoints = 0.01f;
    [SerializeField] private float spacingFromWalls = 0.02f;

    private BuildingsManager buildingsManager;
    private List<Vector3> positionMarkers;
    private Transform upgradeParent;
    private float globalMartWidth;
    private float localMartWidth;

    public float positionMarkerSpan =>
        positionMarkers[positionMarkers.Count - 1].x - positionMarkers[0].x;

    public GymSquareUpgradeLevel UpgradeLevel
    {
        set
        {
            Debug.Log("New upgrade level: " + value.ToString());
            ReturnAllUpgrades();
            if (value == GymSquareUpgradeLevel.OneCenter && buildingsManager.CanGetCenter)
            {
                SpawnUpgrades(() => buildingsManager.GetCenter(Vector3.one * 0.85f),
                    GetSpawnPositions(1, Vector3.up * 0.2f));
            }
            else if (buildingsManager.CanGetMart)
            {
                SpawnUpgrades(
                    () => buildingsManager.GetMart(Vector3.one * globalMartWidth),
                    GetSpawnPositions((int)value));
            }
        }
    }

    private void Awake()
    {
        Transform gymColorQuad = transform.Find("Gym Color Quad");
        upgradeParent = gymColorQuad.Find("Upgrades");
        float globalQuadWidth = gymColorQuad.transform.lossyScale.x;
        float localQuadWidth = gymColorQuad.transform.localScale.x;
        float globalToLocalRatio = globalQuadWidth / localQuadWidth;
        localMartWidth = 
            (localQuadWidth - (2 * spacingFromWalls + 3 * spacingBetweenPoints)) / 4;
        globalMartWidth = localMartWidth * globalToLocalRatio;

        positionMarkers = new List<Vector3>();
        Vector3 startPos = (Vector3.left * localQuadWidth / 2) +
            (Vector3.right * (spacingFromWalls + (localMartWidth / 2))) +
            (Vector3.back * 0.1f);
        Vector3 endPos = startPos.FlippedX();
        Vector3 leftMidpoint = startPos + Vector3.right * 
            (spacingBetweenPoints + localMartWidth);
        Vector3 rightMidpoint = leftMidpoint.FlippedX();
        positionMarkers.AddRange(
            new Vector3[] { startPos, leftMidpoint, rightMidpoint, endPos });
    }

    private void Start()
    {
        buildingsManager = BuildingsManager.Instance;
    }

    private IReadOnlyList<Vector3> GetSpawnPositions(
        int numUpgrades, Vector3? offset = null)
    {
        offset = offset ?? Vector3.zero;
        Vector3 leftPos = positionMarkers[0];
        Vector3 rightPos = positionMarkers[positionMarkers.Count - 1];
        Vector3 midPoint = leftPos.MidPoint(rightPos);
        Vector3 leftMidpoint = leftPos.MidPoint(midPoint);
        Vector3 rightMidpoint = rightPos.MidPoint(midPoint);
        List<Vector3> spawnPositions;
        switch (numUpgrades)
        {
            case 0:
                spawnPositions = new List<Vector3>();
                break;
            case 1:
                spawnPositions = new List<Vector3>(new Vector3[] { midPoint });
                break;
            case 2:
                spawnPositions = new List<Vector3>(
                    new Vector3[] { leftMidpoint, rightMidpoint });
                break;
            case 3:
                spawnPositions = new List<Vector3>(
                    new Vector3[] { leftPos, midPoint, rightPos });
                break;
            case 4:
                spawnPositions = positionMarkers;
                break;
            default:
                throw new ArgumentException("Unsupported number of upgrades");
        }
        return spawnPositions.Select(p => p + offset.Value).ToList();
    }

    private void SpawnUpgrades(Func<GameObject> spawnFunc, 
        IEnumerable<Vector3> spawnPositions)
    {
        foreach (Vector3 spawnPos in spawnPositions)
        {
            GameObject upgrade = spawnFunc();
            upgrade.transform.rotation = upgradeParent.rotation;
            upgrade.transform.parent = upgradeParent;
            upgrade.transform.localPosition = spawnPos;
        }
    }

    private void ReturnAllUpgrades()
    {
        upgradeParent.GetChildren().ForEach(
            c => buildingsManager.ReturnBuilding(c.gameObject));
    }
}
