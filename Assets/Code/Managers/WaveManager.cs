using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Wave[] wavesAvailable;             // contains all waves to choose from, set in inspector
    private Wave currentWave;                                   // contains the currently selected wave to run
    private int currentWaveIndex;                               // wave index
    private int wavesCleared;                                   // counter used to check whether all available waves are cleared
    
    private int totalWaveCount;                                 // number of total waves available
    private float timeBetweenWaves;                             // brief respite between the last cleared wave and the next in seconds
    
    private WaveTimer waveClearTimer;                           // records the total time from start to finish (i.e. boss kill) used for mission ranking
    
    [SerializeField] private List<SpawnPoint> spawnPoints;      // all spawnpoints in the current scene to choose from
    private List<SpawnPoint> currentSpawnPointSelection;        // the current set of n randomly selected spawnpoints
    
    [SerializeField] private List<GameObject> activeEnemies;    // all currently non-dead enemies used to determined when a wave is cleared
    [SerializeField] private Transform enemyTransform;          // just an empty transform containing all spawned enemies in the scene
    
    private void Awake()
    {
        totalWaveCount = wavesAvailable.Length;
        
        // Initialize System with first Wave
        currentWaveIndex = 0;
        currentWave = GetNextWave(currentWaveIndex);
    }

    private void Start()
    {
        // Run First Wave
        RunWave(currentWave);
    }

    private void Update()
    {
        if (!currentWave.isCleared) return;
        
        IncrementWave();
        SetNextWave(currentWaveIndex);
        RunWave(currentWave);
    }
    
    private Wave GetNextWave(int waveNumber)
    {
        return wavesAvailable[waveNumber];
    }
    
    private void SetNextWave(int waveNumber)
    {
        if(waveNumber <= wavesAvailable.Length)
        {
            Debug.Log($"Attempting to set INDEX: {waveNumber} from total {wavesAvailable.Length}");
            currentWave = wavesAvailable[waveNumber -1];
        }
    }
    
    private void RunWave(Wave wave)
    {
        var spawners = GetRandomSpawnPoints();
        Debug.Log(
            $"Starting Wave {currentWaveIndex}! Activating {spawners.Count} SpawnPoints, spawning {currentWave.enemyCountPerSpawnPoint} enemies");
        
        SpawnEnemiesDelayed(wave, spawners);
    }

    private void IncrementWave()
    {
        if (currentWaveIndex < totalWaveCount)
        {
            currentWaveIndex++;
        }
        else
        {
            Debug.Log("All Waves CLEARED!");
        }
    }
    
    private List<SpawnPoint> SelectRandomSpawnPoints(int count)
    {
        return spawnPoints.OrderBy(point => Guid.NewGuid()).Take(count).ToList();
    }

    private List<SpawnPoint> GetRandomSpawnPoints()
    {
        int randomCount = UnityEngine.Random.Range(1, spawnPoints.Count);

        var random = new Random();
        var randomSpawnPoints = new List<SpawnPoint>();
        
        // shuffle the original list
        spawnPoints.OrderBy(point => random.Next());

        for (int i = 0; i < randomCount; i++)
        {
            randomSpawnPoints.Add(spawnPoints[i]);
        }
        
        return randomSpawnPoints;
    }

    private void SpawnEnemiesDelayed(Wave wave, List<SpawnPoint> spawners)
    {
        foreach (var point in spawners)
        {
            point.SpawnMultipleWithDelay(wave, enemyTransform);
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