using System;
using UnityEngine;

[Serializable]
public class WaveTimer : MonoBehaviour
{
    public bool timerActive;
    public bool timerStarted;
    public float currentTime;

    private string currentTimeText;
    private string finalRunTimeText = "NOT SET";

    private TimeSpan runTime;

    public void UpdateWaveTimer()
    {
        if (timerActive) currentTime = currentTime + Time.deltaTime;
        runTime = TimeSpan.FromSeconds(currentTime);
        currentTimeText = FormatTimer();
    }

    public string FormatTimer()
    {
        return runTime.ToString(@"mm\:ss\:ff");
    }

    public void StopTimer()
    {
        timerActive = false;
        GetFinalTime();

        Debug.Log($"Final Time set to: {GetFinalTime()}");

        Debug.Log("<color=red>-----------------Timer stopped!-----------------</color>");
    }

    public void PauseTimer()
    {
        timerActive = false;

        Debug.Log("<color=red>-----------------Timer paused!-----------------</color>");
    }

    public void StartTimer()
    {
        if (!timerStarted)
        {
            currentTime = 0;
            timerStarted = true;
            timerActive = true;

            Debug.Log("<color=magenta>-----------------Timer resumed!-----------------</color>");

            return;
        }

        timerActive = true;

        Debug.Log("<color=yellow>-----------------Timer started!-----------------</color>");
    }

    public string GetFinalTime()
    {
        finalRunTimeText = FormatTimer();
        return finalRunTimeText;
    }
}