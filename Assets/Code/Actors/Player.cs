using System;
using UnityEngine;

public class Player : Actor
{
    [SerializeField] private float damageEffectDuration = 0.25f;

    public static event Action<float> OnPlayerDamage;
    public static event Action OnPlayerDeath;

    protected override void Awake()
    {
        UIManager.OnMissionAccomplished += SetInvincibility;
        base.Awake();
    }

    private void OnDisable()
    {
        UIManager.OnMissionAccomplished -= SetInvincibility;
    }

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