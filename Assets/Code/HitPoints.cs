using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HitPoints : MonoBehaviour
{
    [field: SerializeField] public int MaxHitPoints { get; private set;  } = 20;
    [field: SerializeField] public int CurrentHitPoints { get; private set; } = 20;

    private void ResetHitPoints()
    {
        CurrentHitPoints = MaxHitPoints;
    }
    
    public void IncreaseHitPoints(int amount)
    {
        if (CurrentHitPoints + amount >= MaxHitPoints) CurrentHitPoints += amount;
        else CurrentHitPoints = MaxHitPoints;
    }

    private void DecreaseHitPoints(int amount)
    {
        CurrentHitPoints -= amount;
        if (CurrentHitPoints <= 0) Destroy(gameObject);
    }
  
    private async void DecreaseHitPointsOverTime(int amount, int ticks)
    {
        for (int i = 0; i < ticks; i++)
        {
            DecreaseHitPoints(amount);
            await Task.Delay(1000);
        }
    }
}
