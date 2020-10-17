using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static IReadOnlyList<Transform> GetChildren(this Transform transform)
    {
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }
        return children;
    }

    public static int IndexOf(this Transform transform, Transform child)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) == child) return i;
        }
        return -1;
    }
}
