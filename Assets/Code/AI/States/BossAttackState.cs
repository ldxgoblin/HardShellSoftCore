using DG.Tweening;
using UnityEngine;

public class BossAttackState : AttackState
{
    [SerializeField] private Transform[] projectileTransforms;
    [SerializeField] private GameObject trackerProjectile;
    [SerializeField] private Transform bossHeadTransform;
    [SerializeField] private float telegraphMultiplier = 1.5f;
    
    public override State RunCurrentState()
    {
        if (fireCooldown > 0)
            fireCooldown -= Time.deltaTime;
        else if (fireCooldown <= 0) canFire = true;

        if (canFire && aiDetector.TargetInSight)
        {
            TelegraphAttack();
            
            return this;
        }

        return nextState;
    }

    private void TelegraphAttack()
    {
        bossHeadTransform.DOPunchPosition(new Vector3(1,1), baseFireRate * telegraphMultiplier, 5).OnComplete(RegularFixedAngleAttack);
        StartCooldown();
    }
    
    private void RegularFixedAngleAttack()
    {
        foreach(var transform in projectileTransforms)
        {
            var newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            newProjectile.GetComponent<BossProjectile>().SetupProjectile(transform.right);
        }
    }
    
    private void RotatingStraightAttack()
    {
        var targetDirection = aiDetector.Target.transform.position - transform.position;
        targetDirection.Normalize();

        foreach(var transform in projectileTransforms)
        {
            var newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            newProjectile.GetComponent<BasicEnemyProjectile>().SetupProjectile(transform.right);
        }
    }
}
