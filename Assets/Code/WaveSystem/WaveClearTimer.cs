using System;
using UnityEngine;

[Serializable]
public class WaveClearTimer
{
    public bool timerActive;
    public bool timerStarted;
    public float currentTime;

    public string currentTimeText;
    
    public TimeSpan runTime;

    public void UpdateWaveTimer()
    {
        if (timerActive) currentTime += Time.deltaTime;
        runTime = TimeSpan.FromSeconds(currentTime);
        currentTimeText = FormatTimer();
    }

    public string FormatTimer()
    {
        return runTime.ToString(@"mm\:ss\:ff");
    }

    public void PauseTimer()
    {
        timerActive = false;
    }

    public void StartTimer()
    {
        if (!timerStarted)
        {
            currentTime = 0;
            timerStarted = true;
            timerActive = true;

            return;
        }
        timerActive = true;
    }
}