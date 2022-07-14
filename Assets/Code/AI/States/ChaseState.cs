using UnityEngine;

public class ChaseState : State
{
    [SerializeField] private State nextState;
    [SerializeField] private float chaseSpeed;

    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private AIDetectorCircle aiDetector;

    public override State RunCurrentState()
    {
        if (aiDetector.TargetInSight) Chase();
        return this;
    }

    private void Chase()
    {
        var chaseDirection = aiDetector.Target.transform.position - transform.position;
        var chaseVelocity = chaseDirection * chaseSpeed;
        rigidbody2D.velocity = chaseVelocity * Time.deltaTime;
    }
}