using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChaseFOVState : State
{
    [SerializeField] private State nextState;
    [SerializeField] private State previousState;

    [SerializeField] private float chaseSpeed;
    [MinMaxRange(-15,50)]
    [SerializeField] private RangedFloat chaseSpeedMutationRange;
    
    [SerializeField, Range(0.1f, 0.25f)] private float gapFactor = 0.25f;
    [SerializeField, Range(0f, 1f)] private float decelerationFactor = 0.95f;
    
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private AIDetectorCircle aiDetector;

    [SerializeField] private SpriteRenderer parentSpriteRenderer;

    private float stoppingDistance;
    
    private bool isInAttackRange;
    
    private void Awake()
    {
        var speedVariance = Random.Range(chaseSpeedMutationRange.minValue, chaseSpeedMutationRange.maxValue);
        chaseSpeed += speedVariance;

        stoppingDistance = aiDetector.detectorRadius * gapFactor;
    }

    public override State RunCurrentState()
    {
        if (aiDetector.TargetInSight)
        {
            var distance = Vector2.Distance(aiDetector.Target.transform.position, transform.position);

            if (distance <= aiDetector.detectorRadius && distance >= stoppingDistance)
            {
                ChaseFOV();
            } 
            else if(distance <= stoppingDistance)
            {
                LookAtTarget();
                
                return nextState;
            }

            return this;
        }
        
        return previousState;
    }

    private void ChaseFOV()
    {
        if (aiDetector == null) return;

        var chaseDirection = GetDirectionToTarget();
        
        var chaseVelocity = chaseDirection * chaseSpeed;
        rigidbody2D.AddForce(chaseVelocity, ForceMode2D.Force);
        //rigidbody2D.velocity = chaseVelocity;

        //rotate towards angle of target, but looks weird lol
        //var angle = Mathf.Atan2(chaseDirection.y, chaseDirection.x) * Mathf.Rad2Deg;
        //parentTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        LookAtTarget();
    }

    private Vector3 GetDirectionToTarget()
    {
        var chaseDirection = aiDetector.Target.transform.position - transform.position;
        return chaseDirection.normalized;
    }
    
    private void LookAtTarget()
    {
        var chaseDirection = GetDirectionToTarget();
            
        if (chaseDirection.x > 0)
            parentSpriteRenderer.flipX = false;
        else if (chaseDirection.x < 0) parentSpriteRenderer.flipX = true;
    }
}