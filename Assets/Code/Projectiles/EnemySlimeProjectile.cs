using DG.Tweening;
using UnityEngine;

public class EnemySlimeProjectile : BasicEnemyProjectile
{
    [SerializeField] private float wobbleDuration = 0.25f;
    [SerializeField] private Vector3 maxWobbleScale = Vector3.one;

    protected override void Awake()
    {
        enemyProjectileTransform.DOScale(maxWobbleScale, wobbleDuration).SetLoops(-1, LoopType.Yoyo);
        base.Awake();
    }
}
