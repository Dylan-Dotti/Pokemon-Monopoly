using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionText : Text
{
    private void Update()
    {
        text = "Region: " + (PhotonNetwork.CloudRegion == null ?
            "none" : PhotonNetwork.CloudRegion);
    }
}
