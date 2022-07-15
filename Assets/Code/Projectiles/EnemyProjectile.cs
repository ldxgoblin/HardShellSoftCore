using UnityEngine;

public class EnemyProjectile : BasicProjectile
{
    [SerializeField] private GameObject projectileImpactFX;

    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var playerActor = col.gameObject.GetComponent<Player>();
            playerActor.Damage(projectileDamage);
            
            Destroy(gameObject);
        }

        if (col.gameObject.CompareTag("Mech"))
        {
            var mechActor = col.gameObject.GetComponent<Mech>();
            mechActor.Damage(projectileDamage);
            
            Destroy(gameObject);
        }
    }
}