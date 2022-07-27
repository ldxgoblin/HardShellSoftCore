using System;
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
            
            OnBallStateDamage?.Invoke();

            Destroy(gameObject);
        }

        if (col.gameObject.CompareTag("Mech"))
        {
            var mechActor = col.gameObject.GetComponent<Mech>();

            if(mechActor.MechIsActive)
            {
                mechActor.Damage(projectileDamage);
            
                OnMechStateDamage?.Invoke();
            
                Destroy(gameObject);
            }
        }
    }
}