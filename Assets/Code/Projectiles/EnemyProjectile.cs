using UnityEngine;

public class EnemyProjectile : BasicProjectile
{
    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var playerActor = col.gameObject.GetComponent<Player>();
            playerActor.Damage(projectileDamage);
        }

        if (col.gameObject.CompareTag("Mech"))
        {
            var mechActor = col.gameObject.GetComponent<Mech>();
            mechActor.Damage(projectileDamage);
        }

        base.OnTriggerEnter2D(col);
    }
}