using System;
using UnityEngine;

public class ComboTracker : MonoBehaviour
{
    [SerializeField] private int hitCounter;

    [SerializeField] private float comboTimeWindow = 5;
    [SerializeField] private float currentComboTime;

    private bool comboInProgress;
    public static event Action<int> OnCombo;
    public static event Action OnComboEnded;

    private void Awake()
    {
        Dash.OnDashHit += RegisterHit;
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
                OnComboEnded?.Invoke();
            }
        }
    }

    private void OnDisable()
    {
        Dash.OnDashHit -= RegisterHit;
        PlayerProjectile.OnPlayerProjectileHit -= RegisterHit;
    }
    
    private void RegisterHit()
    {
        if (currentComboTime <= 0) ResetHitCounter();

        hitCounter++;

        if (hitCounter > 1)
        {
            OnCombo?.Invoke(hitCounter);
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