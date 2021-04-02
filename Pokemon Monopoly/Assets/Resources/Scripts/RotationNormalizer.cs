using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RotationNormalizer
{
    public static (float, float) Normalize(float startRotation, float endRotation)
    {
        startRotation = GetPositiveRotation(startRotation);
        endRotation = GetPositiveRotation(endRotation);
        float distance = Mathf.Abs(endRotation - startRotation);
        if (distance >= 180)
        {
            if (endRotation > startRotation)
            {
                endRotation = GetNegativeRotation(endRotation);
            }
            else
            {
                startRotation = GetNegativeRotation(startRotation);
            }
        }
        return (startRotation, endRotation);
    }

    private static float GetPositiveRotation(float rotation)
    {
        return rotation >= 0 ? rotation % 360 : (rotation % -360) + 360;
    }

    private static float GetNegativeRotation(float rotation)
    {
        return rotation > 0 ? (rotation % 360) - 360 : rotation % -360;
    }
}
