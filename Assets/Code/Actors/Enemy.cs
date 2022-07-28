using System;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Actor
{
    [Header("General Values")] [SerializeField]
    private int score = 100;                                        // enemy base score value

    [Header("Splatter VFX")] [SerializeField]
    private GameObject enemyDeathFX;                                // blood splatter vfx prefab

    [Header("Sound FX")]
    [SerializeField] private SimpleAudioEvent enemyDeathAudioEvent; // sploosh!
    
    private CinemachineImpulseSource enemyImpulseSource;            // camera shake
    public static event Action<int> onEnemyKilled;                  // tells UIManager to update its score display
    public static event Action<GameObject> onEnemyDeath;            // tells WaveMananger to delete the current Enemy from its activeEnemies list

    protected override void Awake()
    {
        base.Awake();

        enemyImpulseSource = GetComponent<CinemachineImpulseSource>();
        audioSource = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();
    }

    protected override void Die()
    {
        onEnemyKilled?.Invoke(score);
        onEnemyDeath?.Invoke(gameObject);

        Instantiate(enemyDeathFX, transform.position, Quaternion.identity);
        
        enemyDeathAudioEvent.Play(audioSource);

        enemyImpulseSource.GenerateImpulse(transform.position);

        base.Die();
    }

    public void Bump()
    {
        Vector2 force = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        rigidbody2D.AddForce(force * 100, ForceMode2D.Impulse);
    }
}