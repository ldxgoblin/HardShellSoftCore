using UnityEngine;

public class BossTargetState : State
{
    [SerializeField] private State nextState;
    [SerializeField] private State previousState;

    [MinMaxRange(-15, 50)] [SerializeField]
    private RangedFloat range;
    
    [SerializeField] private AIDetectorCircle aiDetector;

    public override State RunCurrentState()
    {
        if (aiDetector.TargetInSight)
        {
            var distance = Vector2.Distance(aiDetector.Target.transform.position, transform.position);
            Target();
            
            if (distance <= aiDetector.detectorRadius)
            {
                return nextState;
            }
            
            return this;
        }

        return previousState;
    }
    
    private void Target()
    {
        if (aiDetector == null) return;

        var directionToTarget = GetDirectionToTarget();

        //rotate towards angle of target, but looks weird lol
        var angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private Vector3 GetDirectionToTarget()
    {
        var direction = aiDetector.Target.transform.position - transform.position;
        return direction.normalized;
    }
    
}
