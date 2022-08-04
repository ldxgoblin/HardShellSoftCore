using System;
using UnityEngine;

public class Player : Actor
{
    [SerializeField] private float damageEffectDuration = 0.25f;

    public static Action<float> onPlayerDamage;
    public static Action onPlayerDeath;

    public void SetInvincibility(bool state)
    {
        isInvincible = state;
    }

    public override void Damage(int damage)
    {
        onPlayerDamage?.Invoke(damageEffectDuration);
        base.Damage(damage);
    }

    protected override void Die()
    {
        onPlayerDeath?.Invoke();
        base.Die();
    }
    
}