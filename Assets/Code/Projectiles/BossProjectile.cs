using DG.Tweening;
using UnityEngine;

public class BossProjectile : BasicEnemyProjectile
{
    [SerializeField] private Vector3 projectileRotation = Vector3.zero;
    [SerializeField] private float projectileRotationSpeed = 0.25f;
    
    private Transform bossProjectileTransform;
    
    protected override void Awake()
    {
        bossProjectileTransform = transform;

        bossProjectileTransform.DORotate(projectileRotation, projectileRotationSpeed)
            .SetLoops(-1, LoopType.Incremental);
        
        base.Awake();
    }
}
