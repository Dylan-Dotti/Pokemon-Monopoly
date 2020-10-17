using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 MidPoint(this Vector3 vector1, Vector3 vector2)
    {
        return (vector1 + vector2) / 2;
    }

    public static Vector3 FlippedX(this Vector3 vector)
    {
        return new Vector3(-vector.x, vector.y, vector.z);
    }
}
