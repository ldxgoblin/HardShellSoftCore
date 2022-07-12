using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : BasicProjectile
{

    public static event Action OnPlayerProjectileHit;
    
    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            var enemyActor = col.gameObject.GetComponent<Enemy>();
            enemyActor.Damage(projectileDamage);
            
            OnPlayerProjectileHit?.Invoke();
        }
        base.OnTriggerEnter2D(col);
    }
}
