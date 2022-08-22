using System;
using UnityEngine;

public class PlayerProjectile : BasicProjectile
{
    [SerializeField] private GameObject projectileEnemyImpactFX;

    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            var enemyActor = col.gameObject.GetComponent<Actor>();
            if (enemyActor != null)
            {
                Instantiate(projectileEnemyImpactFX, transform.position, transform.rotation);
                enemyActor.Damage(projectileDamage);
                OnPlayerProjectileHit?.Invoke();
            }
        }

        if (col.CompareTag("ChargeZone"))
        {
            return;
            
        }
        base.OnTriggerEnter2D(col);
    }

    public static event Action OnPlayerProjectileHit;
}