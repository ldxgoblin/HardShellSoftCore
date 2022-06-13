using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer),typeof(InputHandler))]
public class Dash : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] private float dashTime = 0.5f;
    [SerializeField, Range(0f, 10f)] private float dashTrailTime = 0.5f;
    [SerializeField, Range(1f, 100f)] private float dashVelocity = 14f;
    
    private InputHandler inputHandler;
    private InputSource inputSource = null;
    
    private Rigidbody2D rigidbody2D;
    private TrailRenderer trailRenderer;

    private Vector2 dashDirection;
    private bool desiredDash;
    private bool isDashing;
    private bool canDash = true;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        if (inputHandler.InputSource != null)
        {
            inputSource = inputHandler.InputSource;
        }
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if(!inputHandler.IsInputActive()) return;
        
        desiredDash = inputHandler.InputSource.GetDashInput();

        if (desiredDash && canDash)
        {
            isDashing = true;
            canDash = false;
            trailRenderer.emitting = true;
            dashDirection = inputHandler.InputSource.GetHorizontalAndVerticalInput();
            
            // if there is no current Input, use direction we are currently facing as dash direction
            if (dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(transform.localScale.x, 0);
            }

            StartCoroutine(StopDashing());
            StartCoroutine(ClearDashTrail());
        }

        if (isDashing)
        {
            PerformDash();
        }
    }
    
    private void PerformDash()
    {
        rigidbody2D.velocity = dashDirection.normalized * dashVelocity;
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        canDash = true;
    }

    private IEnumerator ClearDashTrail()
    {
        yield return new WaitForSeconds(dashTrailTime);
        trailRenderer.emitting = false;
    }
}