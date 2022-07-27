using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool canSpawn;
    public float spawnRate;
    
    public GameObject Spawn(GameObject enemy, Transform parent)
    {
        return Instantiate(enemy, transform.position, Quaternion.identity, parent);
    }

    public void SpawnMultipleWithDelay(Wave wave, Transform parent)
    {
        StartCoroutine(Spawn(wave, parent));
    }

    private IEnumerator Spawn(Wave wave, Transform parent)
    {
        GameObject enemyType = wave.enemyType;
        
        for (int i = 0; i < wave.enemyCountPerSpawnPoint; i++)
        {
            var enemyGo = Instantiate(enemyType, transform.position, Quaternion.identity, parent);
            var enemy = enemyGo.gameObject.GetComponent<Enemy>();
            
            // bump enemy away from spawnpoint to prevent overlapping
            enemy.Bump();

            yield return new WaitForSeconds(spawnRate);
        }

    }
}
