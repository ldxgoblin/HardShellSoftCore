using System.Threading.Tasks;

[System.Serializable]
public class HitPoints
{
    public int maxHitPoints = 20;
    public int currentHitPoints = 20;

    public void ResetHitPoints()
    {
        currentHitPoints = maxHitPoints;
    }
    
    public void IncreaseHitPoints(int amount)
    {
        if (currentHitPoints + amount >= maxHitPoints) currentHitPoints += amount;
        else currentHitPoints = maxHitPoints;
    }

    public void DecreaseHitPoints(int amount)
    {
        currentHitPoints -= amount;
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
