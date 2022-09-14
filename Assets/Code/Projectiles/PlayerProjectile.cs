using System;
using UnityEngine;

public class PlayerProjectile : BasicProjectile
{
    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            var enemyActor = col.gameObject.GetComponent<Actor>();
            if (enemyActor != null)
            {
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