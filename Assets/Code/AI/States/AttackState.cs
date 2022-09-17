using UnityEngine;

public class AttackState : State
{
    [SerializeField] protected State nextState;
    [SerializeField] protected AIDetectorCircle aiDetector;

    [SerializeField] private Transform projectileTransform;
    [SerializeField] protected GameObject projectile;

    [SerializeField] protected float baseFireRate = 1f;
    [SerializeField] protected float fireCooldown;

    [SerializeField] private bool doesContactDamage;

    protected bool canFire;

    public override State RunCurrentState()
    {
        if (fireCooldown > 0)
            fireCooldown -= Time.deltaTime;
        else if (fireCooldown <= 0) canFire = true;

        if (canFire && aiDetector.TargetInSight)
        {
            RegularFixedAngleAttack();
            StartCooldown();

            return this;
        }

        return nextState;
    }

    protected virtual void RegularFixedAngleAttack()
    {
        var targetDirection = aiDetector.Target.transform.position - transform.position;
        targetDirection.Normalize();

        var newProjectile = Instantiate(projectile, projectileTransform.position, Quaternion.identity);

        newProjectile.GetComponent<EnemySlimeProjectile>().SetupProjectile(targetDirection);
    }

    protected void StartCooldown()
    {
        fireCooldown = baseFireRate;
        canFire = false;
    }
}