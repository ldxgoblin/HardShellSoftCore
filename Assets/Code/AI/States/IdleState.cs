using UnityEngine;

public class IdleState : State
{
    [SerializeField] private State nextState;
    [SerializeField] private AIDetectorCircle aiDetector;

    public override State RunCurrentState()
    {
        if (aiDetector.TargetInSight)
        {
            Debug.Log("<color=green>IDLE STATE:</color> Target sighted, switching to <color=yellow>CHASE!</color>");
            return nextState;
        }

        Debug.Log("<color=green>IDLE STATE:</color> No target in sight, remaining <color=green>IDLE!</color>");
        return this;
    }
}