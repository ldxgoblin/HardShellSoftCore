using System;
using UnityEngine;

public class MissionTracker : MonoBehaviour
{
    [SerializeField] private int playerScore;
    [SerializeField] private float playerShotsFired, playerShotsHit;
    [SerializeField] private float playerAccuracy;
    [SerializeField] private float[] accuracyMultipliers =
    {
        1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2
    };
    
    [SerializeField] private int playerMaxCombo, playerDamageDone, playerDamageTaken;

    [SerializeField] private WaveClearTimer timer;
    [SerializeField] private float[] clearTimeMultipliers =
    {
        2f, 1.8f, 1.6f, 1.4f, 1.3f, 1.2f, 1.1f, 1f
    };

    [SerializeField] private int playerScoreBonus;
    public static event Action<int> OnScoreChange;
    
    private void Awake()
    {
        Enemy.OnEnemyKilled += UpdateScore;
        Enemy.OnEnemyHit += UpdatePlayerShotsHit;

        MouseAimAndShoot.OnPlayerShotFired += UpdatePlayerShotsTaken;
        ComboTracker.OnCombo += UpdateMaxCombo;
    }

    private void Update()
    {
        if (!timer.timerStarted) return;
        
        timer.UpdateWaveTimer();
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= UpdateScore;
        Enemy.OnEnemyHit -= UpdatePlayerShotsHit;

        MouseAimAndShoot.OnPlayerShotFired -= UpdatePlayerShotsTaken;
        ComboTracker.OnCombo -= UpdateMaxCombo;
    }
    
    private void UpdateScore(int scoreValue)
    {
        playerScore += scoreValue;

        // notify UiManager
        OnScoreChange?.Invoke(playerScore);
    }
    
    private void UpdateMaxCombo(int combo)
    {
        if (combo > playerMaxCombo) playerMaxCombo = combo;
    }

    private void UpdateAccuracy()
    {
        playerAccuracy = playerShotsHit / playerShotsFired * 100;
    }
    
    private void UpdatePlayerShotsHit(int damage)
    {
        playerShotsHit++;
        UpdatePlayerDamageDone(damage);
    }
    
    private void UpdatePlayerShotsTaken()
    {
        playerShotsFired++;
        UpdateAccuracy();
    }

    private void UpdatePlayerDamageDone(int damage)
    {
        playerDamageDone += damage;
    }
    
    private void RunTimer()
    {
        timer.StartTimer();
    }

    private void CalculateBonusScore()
    {
        
    }
}