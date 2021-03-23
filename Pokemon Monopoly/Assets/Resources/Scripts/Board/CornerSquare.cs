using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CornerSquare : BoardSquare
{
    public override Quaternion GetAvatarMoveRotation()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 playerRotation = currentRotation + Vector3.forward * 45f;
        return Quaternion.Euler(playerRotation);
    }
}
