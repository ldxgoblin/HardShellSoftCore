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
    
    public static event Action<int> onEnemyKilled;

    protected override void Awake()
    {
        enemyImpulseSource = GetComponent<CinemachineImpulseSource>();
        enemyAudioSource = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();
        
        base.Awake();
    }

    protected override void Die()
    {
        onEnemyKilled?.Invoke(score);

        Instantiate(enemyDeathFX, transform.position, Quaternion.identity);
        
        //TODO detach the audiosource from the enemy, otherwise no sound is played because the object its attached to is deactivated
        enemyAudioSource.PlayOneShot(enemyDeathClip);

        enemyImpulseSource.GenerateImpulse(transform.position);

        base.Die();
    }
}