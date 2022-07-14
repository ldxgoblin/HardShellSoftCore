using System;
using UnityEngine;

public class MissionTracker : MonoBehaviour
{
    [SerializeField] private int score;
    
    public static event Action<int> onScoreChange;

    private void Awake()
    {
        Enemy.onEnemyKilled += UpdateScore;
    }

    private void OnDisable()
    {
        Enemy.onEnemyKilled -= UpdateScore;
    }

    private void UpdateScore(int scoreValue)
    {
        score += scoreValue;
        
        // notify UiManager
        onScoreChange?.Invoke(score);
    }
}