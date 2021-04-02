using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Lerper : MonoBehaviour
{
    public Coroutine SpeedLerp(Vector3Lerp lerp, float speed)
    {
        float distance = Vector3.Distance(lerp.StartVector, lerp.EndVector);
        float duration = distance / speed;
        return DurationLerp(lerp, duration);
    }

    public Coroutine DurationLerp(Vector3Lerp lerp, float duration)
    {
        return MultiDurationLerp(new List<Vector3Lerp> { lerp }, duration);
    }

    public virtual Coroutine MultiDurationLerp(
        IEnumerable<Vector3Lerp> lerps, float duration)
    {
        return StartCoroutine(MultiDurationLerpCR(lerps, duration));
    }

    private IEnumerator MultiDurationLerpCR(
        IEnumerable<Vector3Lerp> lerps, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float lerpPercentage = Mathf.Clamp01(
                (Time.time - startTime) / duration);
            foreach (Vector3Lerp lerp in lerps)
            {
                lerp.OnLerpChanged(Vector3.Lerp(
                    lerp.StartVector, lerp.EndVector, lerpPercentage));
            }
            yield return null;
        }
        foreach (Vector3Lerp lerp in lerps)
        {
            lerp.OnLerpChanged(lerp.EndVector);
        }
    }
}

public class Vector3Lerp
{
    public Vector3 StartVector { get; private set; }
    public Vector3 EndVector { get; private set; }
    public Action<Vector3> OnLerpChanged { get; private set; }

    public Vector3Lerp(Vector3 startVector, Vector3 endVector,
        Action<Vector3> onLerpChanged)
    {
        StartVector = startVector;
        EndVector = endVector;
        OnLerpChanged = onLerpChanged;
    }
}
