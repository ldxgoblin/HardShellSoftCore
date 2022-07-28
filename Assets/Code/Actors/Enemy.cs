using System;
using Cinemachine;
using UnityEngine;

public class Enemy : Actor
{
    [Header("General Values")] [SerializeField]
    private int score = 100;

    [Header("Splatter VFX")] [SerializeField]
    private GameObject enemyDeathFX;

    [Header("Sound FX")]
    [SerializeField] private AudioEvent enemyDeathAudioEvent;
    
    private CinemachineImpulseSource enemyImpulseSource;
    
    public static event Action<int> onEnemyKilled;

    protected override void Awake()
    {
        base.Awake();
        
        enemyImpulseSource = GetComponent<CinemachineImpulseSource>();
        audioSource = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();
    }

    protected override void Die()
    {
        onEnemyKilled?.Invoke(score);

        Instantiate(enemyDeathFX, transform.position, Quaternion.identity);
        
        enemyDeathAudioEvent.Play(audioSource);

        enemyImpulseSource.GenerateImpulse(transform.position);

        base.Die();
    }
}