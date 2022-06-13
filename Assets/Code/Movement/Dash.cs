using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{

    [SerializeField] private InputController inputSource = null;
    [SerializeField, Range(0f, 10f)] private float dashTime = 0.5f;
    [SerializeField, Range(0f, 100f)] private float dashVelocity = 14f;
    
    private Rigidbody2D rigidbody2D;
    private TrailRenderer trailRenderer;

    private Vector2 dashDirection;
    private bool desiredDash;
    private bool isDashing;
    private bool canDash = true;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        desiredDash = inputSource.GetDashInput();

        if (desiredDash && canDash)
        {
            isDashing = true;
            canDash = false;
            trailRenderer.emitting = true;
            dashDirection = inputSource.GetHorizontalAndVerticalInput();
            
            // if there is no current Input, use direction we are currently facing as dash direction
            if (dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(transform.localScale.x, 0);
            }

            StartCoroutine(StopDashing());
        }

        if (isDashing)
        {
            PerformDash();
        }
        
        
    }

    private void FixedUpdate()
    {


    }

    private void PerformDash()
    {
        rigidbody2D.velocity = dashDirection.normalized * dashVelocity;
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        isDashing = false;
        canDash = true;
    }

}