using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool canSpawn;
    public float spawnRate;
    
    public GameObject Spawn(GameObject enemy, Transform enemyTransform)
    {
        return Instantiate(enemy, transform.position, Quaternion.identity, enemyTransform);
    }
}
