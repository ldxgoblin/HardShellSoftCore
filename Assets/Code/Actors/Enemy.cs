using System;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Actor
{
    [Header("General Values")] 
    [SerializeField] private int score = 100;                       // enemy base score value
    [Header("Splatter VFX")] 
    [SerializeField] private GameObject enemyDeathFX;               // blood splatter vfx prefab
    [Header("Sound FX")] 
    [SerializeField] private SimpleAudioEvent enemyDeathAudioEvent; // sploosh!

    private CinemachineImpulseSource enemyImpulseSource;            // camera shake
    public static event Action<int> OnEnemyKilled;                  // tells UIManager to update its score display
    public static event Action<GameObject> OnEnemyDeath;            // tells WaveMananger to delete the current Enemy from its activeEnemies list
    public static event Action<int> OnEnemyHit;
    public static event Action<Transform> OnEnemyAddToGroup, OnEnemyRemoveFromGroup;
    
    protected override void Awake()
    {
        base.Awake();

        enemyImpulseSource = GetComponent<CinemachineImpulseSource>();
        audioSource = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();

        OnEnemyAddToGroup?.Invoke(transform);
    }
    
    protected override void Die()
    {
        OnEnemyKilled?.Invoke(score);
        OnEnemyDeath?.Invoke(gameObject);

        Instantiate(enemyDeathFX, transform.position, Quaternion.identity);

        enemyDeathAudioEvent.Play(audioSource);

        enemyImpulseSource.GenerateImpulse(transform.position);

        OnEnemyRemoveFromGroup?.Invoke(transform);

        base.Die();
    }

    public override void Damage(int damage)
    {
        OnOnEnemyHit(damage);
        base.Damage(damage);
    }
    public void Bump()
    {
        var force = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        rigidbody2D.AddForce(force * 100, ForceMode2D.Impulse);
    }

    private static void OnOnEnemyHit(int damage)
    {
        OnEnemyHit?.Invoke(damage);
    }
}