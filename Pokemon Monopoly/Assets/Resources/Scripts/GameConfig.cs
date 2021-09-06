using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    [SerializeField] private int playerStartingMoney = 1500;
    [SerializeField] private int maxNumMarts = 32;
    [SerializeField] private int maxNumCenters = 12;
    [SerializeField] private bool auctionPropertyOnNoBuy = true;

    public static GameConfig Instance { get; private set; }

    public int PlayerStartingMoney
    {
        get => playerStartingMoney;
        set => playerStartingMoney = value;
    }

    public int MaxNumMarts
    {
        get => maxNumMarts;
        set => maxNumMarts = value;
    }

    public int MaxNumCenters
    {
        get => maxNumCenters;
        set => maxNumCenters = value;
    }

    public bool AuctionPropertyOnNoBuy
    {
        get => auctionPropertyOnNoBuy;
        set => auctionPropertyOnNoBuy = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
