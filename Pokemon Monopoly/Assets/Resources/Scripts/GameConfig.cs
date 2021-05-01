using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    [SerializeField] private int playerStartingMoney = 1500;
    [SerializeField] private bool auctionPropertyOnNoBuy = true;

    public static GameConfig Instance { get; private set; }

    public int PlayerStartingMoney => playerStartingMoney;
    public bool AuctionPropertyOnNoBuy => auctionPropertyOnNoBuy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
