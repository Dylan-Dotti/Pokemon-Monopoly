using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Lerper : MonoBehaviour
{
    public Coroutine SpeedLerp(Vector3 startVector, Vector3 targetVector,
        float speed, Action<Vector3> OnLerpChanged)
    {
        float distance = Vector3.Distance(startVector, targetVector);
        float duration = distance / speed;
        return DurationLerp(startVector, targetVector, duration, OnLerpChanged);
    }

    public Coroutine DurationLerp(Vector3 startVector, Vector3 targetVector,
        float duration, Action<Vector3> OnLerpChanged)
    {
        return StartCoroutine(DurationLerpCR(
            startVector, targetVector, duration, OnLerpChanged));
    }

    private IEnumerator DurationLerpCR(Vector3 startVector, Vector3 targetVector,
        float duration, Action<Vector3> OnLerpChanged)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float lerpPercentage = Mathf.Clamp01((Time.time - startTime) / duration);
            OnLerpChanged(Vector3.Lerp(startVector, targetVector, lerpPercentage));
            yield return null;
        }
        OnLerpChanged(targetVector);
    }
}
