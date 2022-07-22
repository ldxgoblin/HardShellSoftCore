using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}

[Serializable]
public class Wave
{
    public List<GameObject> enemyTypes;
    public int numberOfEnemiesToSpawn;
    public WaveTimer timer;
}