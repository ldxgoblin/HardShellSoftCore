using System;
using Cinemachine;
using UnityEngine;

public class Enemy : Actor
{
    [Header("General Values")] [SerializeField]
    private int score = 100;

    [Header("Splatter VFX")] [SerializeField]
    private GameObject enemyDeathFX;

    [Header("Sound FX")] [SerializeField] private AudioClip enemyDeathClip;

    private AudioSource enemyAudioSource;
    private CinemachineImpulseSource enemyImpulseSource;

    protected override void Awake()
    {
        enemyImpulseSource = GetComponent<CinemachineImpulseSource>();
        enemyAudioSource = GetComponent<AudioSource>();
        base.Awake();
    }

    public static event Action<int> onEnemyKilled;

    protected override void Die()
    {
        onEnemyKilled?.Invoke(score);

        Instantiate(enemyDeathFX, transform.position, Quaternion.identity);
        enemyAudioSource.PlayOneShot(enemyDeathClip);

        enemyImpulseSource.GenerateImpulse(transform.position);

        base.Die();
    }
}