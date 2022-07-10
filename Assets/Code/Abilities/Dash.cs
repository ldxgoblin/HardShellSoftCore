using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer),typeof(InputHandler))]
public class Dash : MonoBehaviour
{
    [SerializeField] private int maxDashCount = 2;
    [SerializeField, Range(0.25f, 1f)] private float dashTime = 0.5f;
    [SerializeField, Range(150f, 250f)] private float dashSpeed = 50f;
    [SerializeField, Range(1, 10)] private int dashDamage = 1;
    [SerializeField] private int damageFrames = 10;

    private InputHandler inputHandler;
    private InputSource inputSource;
    
    private Rigidbody2D rigidbody2D;
    private GroundCheck groundCheck;
    private TrailRenderer trailRenderer;

    private Vector2 dashDirection;
    private bool desiredDash;
    
    private bool isDashing;
    private bool isOnGround;
    
    private bool canDash = true;
    private bool canDamage = true;
    
    private int dashPhase = 0;
    
    [SerializeField] private AudioClip _dashClip;
    private AudioSource audioSource;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        if (inputHandler.InputSource != null)
        {
            inputSource = inputHandler.InputSource;
        }
        
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<GroundCheck>();
        trailRenderer = GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!inputHandler.IsInputActive()) return;
        
        desiredDash = inputHandler.InputSource.GetDashInput();
        
        if (desiredDash)
        {   
            // get and store direction in the exact moment the input is true
            dashDirection = GetDashDirection();
        }
        
        isOnGround = groundCheck.GetCurrentGroundState();
        
        // reset dash counter
        if (isOnGround)
        {
            dashPhase = 0;
        }
        
        if (desiredDash && canDash)
        {
            if (dashPhase < maxDashCount)
            {
                dashPhase += 1;
                isDashing = true;
                canDash = false;
            }
            EndDash();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            PerformDash();
        }
    }

    private Vector2 GetDashDirection()
    {
        Vector2 dashDirection = CrossHair.GetCrossHairPosition() - transform.position;
        return dashDirection.normalized;
    }
    
    private void PerformDash()
    {
        trailRenderer.emitting = true;

        Vector2 dashVelocity = dashDirection * dashSpeed;
        rigidbody2D.MovePosition(rigidbody2D.position + dashVelocity * Time.fixedDeltaTime);
        
        StartCoroutine(DamageFrames(damageFrames));
    }

    private void EndDash()
    {
        StartCoroutine(StopDashDelayed());
        StartCoroutine(ClearDashTrailDelayed());
    }
    
    private IEnumerator StopDashDelayed()
    {
        yield return new WaitForSeconds(dashTime);
        StopDashInstantly();
    }

    private void StopDashInstantly()
    {
        isDashing = false;
        canDash = true;
    }

    private IEnumerator DamageFrames(int frames)
    {
        canDamage = true;
        
        for (int i = 0; i < frames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        
        canDamage = false;
    }
    
    private IEnumerator ClearDashTrailDelayed()
    {
        yield return new WaitForSeconds(dashTime);
        ClearDashTrailInstantly();
    }

    private void ClearDashTrailInstantly()
    {
        trailRenderer.emitting = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            if (canDamage)
            {
                var enemyActor = col.gameObject.GetComponent<Enemy>();
                enemyActor.Damage(dashDamage);
            }

            StopDashInstantly();
        }
    }
}