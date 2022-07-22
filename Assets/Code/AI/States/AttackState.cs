using UnityEngine;

public class AttackState : State
{
    [SerializeField] private State nextState;
    [SerializeField] private AIDetectorCircle aiDetector;

    [SerializeField] private Transform projectileTransform;
    [SerializeField] private GameObject projectile;

    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float fireCooldown;

    [SerializeField] private bool doesContactDamage;
    private bool canFire;

    public override State RunCurrentState()
    {
        if (fireCooldown > 0)
            fireCooldown -= Time.deltaTime;
        else if (fireCooldown <= 0) canFire = true;

        if (canFire && aiDetector.TargetInSight)
        {
            // Debug.Log("<color=red>ATTACK STATE:</color> Target in Range, attacking!");
            Attack();
            StartCooldown();

            return this;
        }

        // Debug.Log("<color=red>ATTACK STATE:</color> Target out of Range, switching to <color=yellow>CHASE!</color>");
        return nextState;
    }

    private void Attack()
    {
        var targetDirection = aiDetector.Target.transform.position - transform.position;
        var newProjectile = Instantiate(projectile, projectileTransform.position, Quaternion.identity);

        newProjectile.GetComponent<EnemyProjectile>().SetupProjectile(targetDirection);
    }

    private void StartCooldown()
    {
        fireCooldown = fireRate;
        canFire = false;
    }
}