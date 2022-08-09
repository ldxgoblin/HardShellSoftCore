using System;
using DG.Tweening;
using UnityEngine;

public class BossEnemy : Actor
{
    [SerializeField] private int score;
    
    private Transform bossHeadTransform;
    private Vector3 bossHeadBaseScale;
    public static event Action<int> OnBossHit;
    public static event Action<HitPoints> OnBossHitPointUpdate; 
    public static event Action OnBossKilled;
    public static event Action OnBossSpawned;
    public static event Action<int> OnBossScore;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        OnBossHitPointUpdate?.Invoke(HitPoints);
        OnBossSpawned?.Invoke();
        
        bossHeadTransform = GetComponent<Transform>();
        bossHeadBaseScale = bossHeadTransform.localScale;

        base.Awake();
    }

    protected void Start()
    {
        bossHeadTransform = GetComponent<Transform>();
    }
    
    public override void Damage(int damage)
    {
        OnBossHit?.Invoke(damage);
        OnBossHitPointUpdate?.Invoke(HitPoints);
        
        base.Damage(damage);
    }

    protected override void Die()
    {
        OnBossKilled?.Invoke();
        OnBossScore?.Invoke(score);
        base.Die();
    }
}
