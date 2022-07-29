using System.Collections;
using UnityEngine;

public class SlowMotion : MonoBehaviour
{
    private bool waiting;

    public void SlowDown(float duration, float timeScale)
    {
        if (waiting)
            return;
        Time.timeScale = timeScale;
        StartCoroutine(Wait(duration));
    }

    public void SlowDown(float duration)
    {
        SlowDown(duration, 0.0f);
    }

    private IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
        waiting = false;
    }
}