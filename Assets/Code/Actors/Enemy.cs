using System;
using UnityEngine;

public class Enemy : Actor
{
    [SerializeField] private int score = 100;

    public static event Action<int> onEnemyKilled;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        onEnemyKilled?.Invoke(score);
        base.Die();
    }
    
}
