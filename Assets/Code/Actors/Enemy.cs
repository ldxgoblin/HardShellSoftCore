using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Actor
{
    [SerializeField] private int score = 100;                       // enemy base score value
    [SerializeField] private GameObject enemyDeathFX;               // blood splatter vfx prefab
    [SerializeField] private GameObject enemyHitFX; 
    [SerializeField] private SimpleAudioEvent enemyDeathAudioEvent; // sploosh!
    private Transform enemyTransform;
    private Vector3 enemyBaseScale;
    
    private CinemachineImpulseSource enemyImpulseSource;            // camera shake
    public static event Action<int> OnEnemyKilled;                  // tells UIManager to update its score display
    public static event Action<GameObject> OnEnemyDeath;            // tells WaveMananger to delete the current Enemy from its activeEnemies list
    public static Action<int> onEnemyHit;
    public static event Action<Transform> OnEnemyAddToGroup, OnEnemyRemoveFromGroup;
    
    protected override void Awake()
    {
        base.Awake();

        enemyImpulseSource = GetComponent<CinemachineImpulseSource>();
        audioSource = GameObject.FindWithTag("AudioPlayer").GetComponent<AudioSource>();

        OnEnemyAddToGroup?.Invoke(transform);

        enemyTransform = transform;
        enemyBaseScale = enemyTransform.localScale;
    }

    protected virtual void Start()
    {
        enemyTransform.DOScaleY(2f, 0.7f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    protected override void Die()
    {
        OnEnemyKilled?.Invoke(score);
        OnEnemyDeath?.Invoke(gameObject);
        
        enemyDeathAudioEvent.Play(audioSource);
        enemyImpulseSource.GenerateImpulse(transform.position);
        OnEnemyRemoveFromGroup?.Invoke(transform);

        Instantiate(enemyDeathFX, enemyTransform.position, Quaternion.identity);

        base.Die();
    }

    public override void Damage(int damage)
    {
        onEnemyHit?.Invoke(damage);
        
        Instantiate(enemyHitFX, enemyTransform.position, Quaternion.identity);
        
        var wobbleSequence = DOTween.Sequence();
        wobbleSequence.Append(enemyTransform.DOPunchScale(new Vector3(0.35f, 0.35f, 0.35f), 0.25f))
            .Append(enemyTransform.DOScale(enemyBaseScale, 0.25f));

        base.Damage(damage);
    }
    public void Bump()
    {
        var force = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
        rigidbody2D.AddForce(force * 100, ForceMode2D.Impulse);
    }
}