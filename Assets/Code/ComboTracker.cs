using System;
using UnityEngine;

public class ComboTracker : MonoBehaviour
{
    [SerializeField] private int hitCounter;

    [SerializeField] private float comboTimeWindow = 5;
    [SerializeField] private float currentComboTime;

    private bool comboInProgress;

    private void Awake()
    {
        Dash.onDashHit += RegisterHit;
        PlayerProjectile.OnPlayerProjectileHit += RegisterHit;

        ResetHitCounter();
    }

    private void Update()
    {
        if (comboInProgress)
        {
            if (currentComboTime > 0)
            {
                currentComboTime -= Time.deltaTime;
            }
            else
            {
                comboInProgress = false;
                onComboEnded?.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        Dash.onDashHit -= RegisterHit;
        PlayerProjectile.OnPlayerProjectileHit -= RegisterHit;
    }

    public static event Action<int> onCombo;
    public static event Action onComboEnded;

    private void RegisterHit()
    {
        if (currentComboTime <= 0) ResetHitCounter();

        hitCounter++;

        if (hitCounter > 1)
        {
            onCombo?.Invoke(hitCounter);
            comboInProgress = true;
        }
    }

    private void UpdateScore(int multiplier)
    {
        // add base score value multiplied by hitCounter to Score UI
    }

    private void ResetHitCounter()
    {
        comboInProgress = false;
        hitCounter = 0;
        currentComboTime = comboTimeWindow;
    }
}