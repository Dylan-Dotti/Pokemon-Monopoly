using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropertyManager : MonoBehaviour
{
    public static PropertyManager Instance { get; private set; }

    private LeagueData[] leagues;
    private List<PropertyData> properties;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            properties = new List<PropertyData>();
            var leaguesInstantiated = new List<LeagueData>();
            var leaguePrefabs = Resources.LoadAll<LeagueData>(
                "Scriptable Objects/Leagues");
            Debug.Log("Num league prefabs: " + leaguePrefabs.Length);
            foreach (LeagueData league in leaguePrefabs)
            {
                LeagueData leagueDataCopy = Instantiate(league);
                leaguesInstantiated.Add(leagueDataCopy);
                foreach (PropertyCollection collection in leagueDataCopy.PropertyCollections)
                {
                    properties.AddRange(collection.Properties);
                }
            }
            leagues = leaguesInstantiated.ToArray();
        }
    }

    public LeagueData GetLeagueByName(string leagueName)
    {
        return leagues.Where(l => l.LeagueName == leagueName).FirstOrDefault();
    }

    public PropertyData GetPropertyByName(string name)
    {
        return properties.Where(p => p.PropertyName == name).FirstOrDefault();
    }

    public IReadOnlyList<PropertyData> GetAllProperties()
    {
        return properties;
    }
}
