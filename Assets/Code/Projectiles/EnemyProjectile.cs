using System;
using Unity.Mathematics;
using UnityEngine;

public class EnemyProjectile : BasicProjectile
{
    [SerializeField] private GameObject projectileImpactFX;
    public static event Action OnMechStateDamage;
    public static event Action OnBallStateDamage;
    
    public override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var playerActor = col.gameObject.GetComponent<Player>();

            playerActor.Damage(projectileDamage);

            Instantiate(projectileImpactFX, transform.position, quaternion.identity);
            
            OnBallStateDamage?.Invoke();
                
            Destroy(gameObject);
        }

        if (col.gameObject.CompareTag("Mech"))
        {
            var mechActor = col.gameObject.GetComponent<Mech>();

            if (mechActor.MechIsActive)
            {
                mechActor.Damage(projectileDamage);

                Instantiate(projectileImpactFX, transform.position, quaternion.identity);
                
                OnMechStateDamage?.Invoke();

                Destroy(gameObject);
            }
        }
    }

    protected override void AutoDestruct()
    {
        Instantiate(projectileImpactFX, transform.position, quaternion.identity);
        base.AutoDestruct();
    }
}