using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class WaveManager : MonoBehaviour
{
    private int waveCount;
    private float secondsBetweenWaves;
    
    private WaveTimer waveTimer;
    
    [SerializeField] private List<SpawnPoint> spawnPoints;
    [SerializeField] private List<GameObject> activeEnemies;
    [SerializeField] private Transform enemyTransform;

    [SerializeField] private Wave[] waves;
    
    private int currentWaveNumber;

    private Wave currentWave;
    
    private void Awake()
    {
        waveCount = waves.Length;
        
        currentWaveNumber = 0;
        currentWave = waves[currentWaveNumber];
    }

    private void Start()
    {
        // First Wave
        RunWave(currentWave);
    }

    private void Update()
    {

        if (!currentWave.isCleared) return;
        
        // Start next Wave
        currentWaveNumber++;
        currentWave = GetNextWave(currentWaveNumber);
        var spawners = GetRandomSpawnPoints(currentWaveNumber);
        SpawnEnemies(currentWave, spawners);
    }
    
    private Wave GetNextWave(int waveNumber)
    {
        return waves[waveNumber];
    }
    
    private void RunWave(Wave wave)
    {
        currentWaveNumber++;
        var spawners = GetRandomSpawnPoints(currentWaveNumber);
        Debug.Log(
            $"Starting Wave {currentWave}! Activating {spawners.Count} SpawnPoints, spawning {currentWave.enemyCountPerSpawnPoint} enemies");
        SpawnEnemies(wave, spawners);
    }

    private List<SpawnPoint> SelectRandomSpawnPoints(int count)
    {
        return spawnPoints.OrderBy(point => Guid.NewGuid()).Take(count).ToList();
    }

    private List<SpawnPoint> GetRandomSpawnPoints(int count)
    {
        var random = new Random();
        var randomSpawnPoints = new List<SpawnPoint>();
        
        // shuffle the original list
        spawnPoints.OrderBy(point => random.Next());

        for (int i = 0; i < count; i++)
        {
            randomSpawnPoints.Add(spawnPoints[i]);
        }
        
        return randomSpawnPoints;
    }
    
    private void SpawnEnemies(Wave wave, List<SpawnPoint> spawners)
    {
        foreach (var point in spawners)
        {
            for (int i = 0; i < wave.enemyCountPerSpawnPoint; i++)
            {
                var enemy = point.Spawn(wave.enemyType, enemyTransform);
                activeEnemies.Add(enemy);
            }
        }
    }
}

[Serializable]
public class Wave
{
    public GameObject enemyType;
    public int enemyCountPerSpawnPoint;

    public bool isCleared;
}