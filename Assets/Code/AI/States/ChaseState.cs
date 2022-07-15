using UnityEngine;

public class ChaseState : State
{
    [SerializeField] private State nextState;
    [SerializeField] private State previousState;

    [SerializeField] private float chaseSpeed;

    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private AIDetectorCircle aiDetector;

    [SerializeField] private Transform parentTransform;
    [SerializeField] private SpriteRenderer parentSpriteRenderer;

    public override State RunCurrentState()
    {
        if (aiDetector.TargetInSight)
        {
            Debug.Log("<color=yellow>CHASE STATE:</color> Target in sight, closing distance!");
            Chase();
            var distance = Vector2.Distance(aiDetector.Target.transform.position, transform.position);

            if (distance <= aiDetector.detectorRadius / 2)
            {
                Debug.Log("<color=yellow>CHASE STATE:</color> switching to <color=red>ATTACK!</color>");
                return nextState;
            }

            return this;
        }

        // switch back to idle if target is lost
        Debug.Log("<color=yellow>CHASE STATE:</color> Target lost, switching to <color=green>IDLE</color>");
        return previousState;
    }

    private void Chase()
    {
        var chaseDirection = aiDetector.Target.transform.position - transform.position;

        var chaseVelocity = chaseDirection * chaseSpeed;
        rigidbody2D.velocity = chaseVelocity * Time.deltaTime;

        //rotate towards angle of target, but looks weird lol
        //var angle = Mathf.Atan2(chaseDirection.y, chaseDirection.x) * Mathf.Rad2Deg;
        //parentTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (chaseDirection.x > 0)
            parentSpriteRenderer.flipX = false;
        else if (chaseDirection.x < 0) parentSpriteRenderer.flipX = true;
    }
}