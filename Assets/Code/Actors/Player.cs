using System;
using UnityEngine;

public class Player : Actor
{
    [SerializeField] private float damageEffectDuration = 0.25f;

    public static event Action<float> OnPlayerDamage;
    public static event Action OnPlayerDeath;

    public void SetInvincibility(bool state)
    {
        isInvincible = state;
    }

    public override void Damage(int damage)
    {
        OnPlayerDamage?.Invoke(damageEffectDuration);
        base.Damage(damage);
    }

    protected override void Die()
    {
        OnPlayerDeath?.Invoke();
        base.Die();
    }
    
}