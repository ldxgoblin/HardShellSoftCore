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
                
                Destroy(gameObject);
            }
        }
    }

    public static event Action OnPlayerProjectileHit;
}