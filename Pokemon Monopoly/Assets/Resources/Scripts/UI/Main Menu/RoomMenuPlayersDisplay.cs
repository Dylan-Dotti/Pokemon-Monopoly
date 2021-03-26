using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMenuPlayersDisplay : MonoBehaviour
{
    [SerializeField] private RoomPlayerDisplay playerDisplayPrefab;
    [SerializeField] private PhotonRoom room;

    private void Awake()
    {
        room.NumPlayersChanged += OnRoomNumPlayersChanged;
    }

    private void OnEnable()
    {
        SpawnPlayerDisplays();
    }

    private void OnRoomNumPlayersChanged(int lastValue, int newValue) 
    {
        SpawnPlayerDisplays();
    }

    private void DestroyPlayerDisplays()
    {
        foreach (Transform t in transform.GetChildren())
        {
            Destroy(t.gameObject);
        }
    }

    private void SpawnPlayerDisplays()
    {
        DestroyPlayerDisplays();
        foreach (Player p in room.PlayersInRoom)
        {
            var playerDisplay = Instantiate(playerDisplayPrefab, transform);
            playerDisplay.PlayerName = p.NickName;
        }
    }
}
