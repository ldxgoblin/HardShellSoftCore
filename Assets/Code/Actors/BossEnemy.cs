using System;
using DG.Tweening;
using UnityEngine;

public class BossEnemy : Actor
{
    public static event Action<int> OnBossHit; 
    private Transform bossHeadTransform;
    private Vector3 bossHeadBaseScale;

    public static event Action OnBossKilled;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
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
        
        var wobbleSequence = DOTween.Sequence();
        wobbleSequence.Append(bossHeadTransform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.15f))
            .Append(bossHeadTransform.DOScale(bossHeadBaseScale, 0.25f));

        base.Damage(damage);
    }

    protected override void Die()
    {
        OnBossKilled?.Invoke();
        base.Die();
    }
}
