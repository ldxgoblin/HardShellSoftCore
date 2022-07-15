using System;
using UnityEngine;

public class PlayerProjectile : BasicProjectile
{
    [SerializeField] private GameObject projectileEnemyImpactFX;

    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            var enemyActor = col.gameObject.GetComponent<Enemy>();
            Instantiate(projectileEnemyImpactFX, transform.position, transform.rotation);
            enemyActor.Damage(projectileDamage);

            OnPlayerProjectileHit?.Invoke();
        }

        base.OnTriggerEnter2D(col);
    }

    public static event Action OnPlayerProjectileHit;
}