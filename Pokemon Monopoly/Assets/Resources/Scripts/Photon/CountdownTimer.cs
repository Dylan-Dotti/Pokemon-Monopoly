using System;
using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    public event UnityAction<float> SecondsRemainingChanged;
    public event UnityAction CountdownCompleted;

    private float defaultSeconds;
    private float currentSeconds;

    private void Update()
    {
        currentSeconds -= Time.deltaTime;
        SecondsRemainingChanged?.Invoke(currentSeconds);
        if (currentSeconds <= 0)
        {
            CountdownCompleted?.Invoke();
            StopCountdown();
        }
    }

    public void StartCountdown(int numSeconds)
    {
        StopCountdown();
        if (numSeconds < 1)
        {
            throw new ArgumentException(
                "numSeconds can't be less than 1");
        }
        currentSeconds = defaultSeconds = numSeconds;
        enabled = true;
        SecondsRemainingChanged?.Invoke(currentSeconds);
    }

    public void StopCountdown()
    {
        enabled = false;
    }
}
