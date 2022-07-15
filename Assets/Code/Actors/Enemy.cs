using System;
using UnityEngine;

public class Enemy : Actor
{
    [Header("General Values")] [SerializeField]
    private int score = 100;

    [Header("Splatter VFX")] [SerializeField]
    private GameObject enemyDeathFX;

    [Header("Sound FX")] [SerializeField] private AudioClip enemyDeathClip;

    private AudioSource enemyAudioSource;

    protected override void Awake()
    {
        enemyAudioSource = GetComponent<AudioSource>();
        base.Awake();
    }

    public static event Action<int> onEnemyKilled;

    public override void Die()
    {
        onEnemyKilled?.Invoke(score);

        Instantiate(enemyDeathFX, transform.position, Quaternion.identity);
        enemyAudioSource.PlayOneShot(enemyDeathClip);

        base.Die();
    }
}