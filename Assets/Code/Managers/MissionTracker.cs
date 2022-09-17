using System;
using UnityEngine;

public class MissionTracker : MonoBehaviour
{
    [SerializeField] private int playerScore;
    [SerializeField] private float playerShotsFired, playerShotsHit;
    [SerializeField] private float playerAccuracy;
    
    [SerializeField] private int playerMaxCombo, playerDamageDone, playerDamageTaken;

    [SerializeField] private WaveClearTimer timer;
    
    [SerializeField] private int playerScoreBonus;
    
    [SerializeField] private float[] clearTimeMultipliers =
    {
        2f, 1.8f, 1.6f, 1.4f, 1.3f, 1.2f, 1.1f, 1f
    };
    [SerializeField] private float[] accuracyMultipliers =
    {
        1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f, 1.7f, 1.8f, 1.9f, 2
    };
    public static event Action<int> OnScoreChange;
    public static event Action<int, int> OnScoreSubmission; 

    public static event Action<int, float, int, int, int, string, string> OnBlastAllStatistics;

    private void Awake()
    {
        Enemy.OnEnemyKilled += UpdateScore;
        Enemy.onEnemyHit += TrackPlayerShotsHit;

        BossEnemy.OnBossHit += TrackPlayerShotsHit;

        Player.OnPlayerDeath += PauseTimer;
        Player.OnPlayerDeath += EndTracking;
        
        MouseAimAndShoot.OnPlayerShotFired += TrackPlayerShotsTaken;
        Dash.OnDash += TrackPlayerShotsTaken;
        Dash.OnDashDamage += TrackPlayerShotsHit;
        
        ComboTracker.OnCombo += TrackMaxCombo;

        WaveManager.OnStartTrackingWaveTime += RunTimer;
        WaveManager.OnStopTrackingWaveTime += PauseTimer;
        
        BossEnemy.OnBossKilled += EndTracking;
        BossEnemy.OnBossKilled += PauseTimer;
        BossEnemy.OnBossScore += UpdateScore;
    }

    private void Update()
    {
        if (!timer.timerStarted) return;
        
        timer.UpdateWaveTimer();
    }

    private void OnDisable()
    {
        Enemy.OnEnemyKilled -= UpdateScore;
        Enemy.onEnemyHit -= TrackPlayerShotsHit;
        
        BossEnemy.OnBossHit -= TrackPlayerShotsHit;
        
        Player.OnPlayerDeath -= PauseTimer;
        Player.OnPlayerDeath -= EndTracking;

        MouseAimAndShoot.OnPlayerShotFired -= TrackPlayerShotsTaken;
        Dash.OnDash -= TrackPlayerShotsTaken;
        Dash.OnDashDamage -= TrackPlayerShotsHit;
        
        ComboTracker.OnCombo -= TrackMaxCombo;
        
        WaveManager.OnStartTrackingWaveTime -= RunTimer;
        WaveManager.OnStopTrackingWaveTime -= PauseTimer;

        BossEnemy.OnBossKilled -= EndTracking;
        BossEnemy.OnBossKilled -= PauseTimer;
        BossEnemy.OnBossScore -= UpdateScore;
    }
    
    private void UpdateScore(int scoreValue)
    {
        playerScore += scoreValue;

        // notify UiManager
        OnScoreChange?.Invoke(playerScore);
    }
    
    private void TrackMaxCombo(int combo)
    {
        if (combo > playerMaxCombo) playerMaxCombo = combo;
    }

    private void TrackAccuracy()
    {
        playerAccuracy = Mathf.Round(playerShotsHit / playerShotsFired * 100);
    }
    
    private void TrackPlayerShotsHit(int damage)
    {
        playerShotsHit++;
        TrackPlayerDamageDone(damage);
    }
    
    private void TrackPlayerShotsTaken()
    {
        playerShotsFired++;
        TrackAccuracy();
    }

    private void TrackPlayerDamageDone(int damage)
    {
        playerDamageDone += damage;
    }
    
    private void RunTimer()
    {
        timer.StartTimer();
    }

    private void PauseTimer()
    {
        timer.PauseTimer();
    }

    private void EndTracking()
    {
        playerScoreBonus = CalculateBonusScore();
        var totalScore = playerScore + playerScoreBonus;
        
        OnBlastAllStatistics?.Invoke(playerDamageDone, playerAccuracy, playerMaxCombo, totalScore, playerScoreBonus, timer.currentTimeText, GetRank(totalScore));
        
        OnScoreSubmission?.Invoke(playerScore, playerScoreBonus);
    }
    private int CalculateBonusScore()
    {
        var damageBonus = Mathf.Round(playerDamageDone * GetAccuracyMultiplier(playerAccuracy));
        var comboBonus = playerMaxCombo * 10 / (playerAccuracy * 0.01f);
        var clearTimeBonus = playerDamageDone * GetClearTimeMultiplier(timer.currentTime);

        var sum = damageBonus + comboBonus + clearTimeBonus;

        if (sum < 0) return 0;
        
        return Mathf.RoundToInt(damageBonus + comboBonus + clearTimeBonus);
    }

    private float GetAccuracyMultiplier(float accuracy)
    {
        // TODO get multipliers from array instead of hardcoding
        return accuracy switch
        {
            <= 1 => 1f,
            >= 5 and < 11 => 1.1f,
            >= 11 and < 19 => 1.2f,
            >= 19 and < 34 => 1.3f,
            >= 34 and < 52 => 1.4f,
            >= 52 and < 69 => 1.5f,
            >= 69 and < 82 => 1.6f,
            >= 82 and < 90 => 1.7f,
            >= 90 and < 96 => 1.8f,
            >= 96 and < 100 => 1.9f,
            _ => accuracy >= 100 ? 2f : 1f
        };
    }
    
    private float GetClearTimeMultiplier(float time)
    {
        // TODO get multipliers from array instead of hardcoding
        return time switch
        {
            < 30 => 2f,
            >= 30 and < 60 => 1.8f,
            >= 60 and < 90 => 1.6f,
            >= 90 and < 120 => 1.4f,
            >= 120 and < 150 => 1.3f,
            >= 150 and < 180 => 1.2f,
            >= 180 and < 240 => 1.1f,
            >= 240 => 1f,
            _ => 1f
        };
    }
    
    private string GetRank(int totalScore)
    {
        return totalScore switch
        {
            < 2500 => "F",
            >= 2500 and < 10000 => "E",
            >= 10000 and < 20000 => "D",
            >= 20000 and < 35000 => "C",
            >= 35000 and < 55000 => "B",
            >= 55000 and < 85000 => "A",
            >= 85000 and < 130000 => "S",
            _ => "X"
        };
    }
}