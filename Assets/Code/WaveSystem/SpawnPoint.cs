using System;
using System.Collections;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float spawnRate;
    public static event Action<GameObject> onEnemyBirth;

    public GameObject Spawn(GameObject enemy, Transform parent)
    {
        return Instantiate(enemy, transform.position, Quaternion.identity, parent);
    }

    public void SpawnMultipleWithDelay(Wave wave, Transform parent, float rate)
    {
        StartCoroutine(Spawn(wave, parent, rate));
    }

    private IEnumerator Spawn(Wave wave, Transform parent, float rate)
    {
        var enemyType = wave.enemyType;

        for (var i = 0; i < wave.enemyCountPerSpawnPoint; i++)
        {
            // TODO Object Pooling
            var enemyGo = Instantiate(enemyType, transform.position, Quaternion.identity, parent);
            var enemy = enemyGo.gameObject.GetComponent<Enemy>();

            // bump enemy away from spawnpoint to prevent overlapping
            enemy.Bump();

            onEnemyBirth?.Invoke(enemyGo);

            yield return new WaitForSeconds(rate);
        }
    }
}