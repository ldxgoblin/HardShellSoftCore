using UnityEngine;

public class IdleState : State
{
    [SerializeField] private State nextState;
    [SerializeField] private AIDetectorCircle aiDetector;

    public override State RunCurrentState()
    {
        if (aiDetector.TargetInSight) return nextState;

        return this;
    }
}