using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare : MonoBehaviour
{
    public float Width => transform.lossyScale.x;
    public float Height => transform.lossyScale.y;
}
