using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionLerper : MonoBehaviour
{
    public Coroutine SpeedLerp(Vector3 targetPos, float speed)
    {
        return StartCoroutine(SpeedLerpCR(targetPos, speed));
    }

    private IEnumerator SpeedLerpCR(Vector3 targetPos, float speed)
    {
        Vector3 startPos = transform.position;
        float distance = Vector3.Distance(startPos, targetPos);
        float duration = distance / speed;
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float lerpPercentage = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(
                startPos, targetPos, lerpPercentage);
            yield return null;
        }
    }
}
