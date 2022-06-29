using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer),typeof(InputHandler))]
public class Dash : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] private float dashTime = 0.5f;
    [SerializeField, Range(0f, 10f)] private float dashTrailTime = 0.5f;
    [SerializeField, Range(50f, 200f)] private float dashVelocity = 50f;

    [SerializeField, Range(1, 10)] private int dashDamage = 1;
    [SerializeField] private int damageFrames = 10;
    
    private InputHandler inputHandler;
    private InputSource inputSource = null;
    
    private Rigidbody2D rigidbody2D;
    private GroundCheck groundCheck;
    private TrailRenderer trailRenderer;

    private Vector2 dashDirection;
    private bool desiredDash;
    private bool isDashing;
    private bool isOnGround;
    private bool canDash = true;
    
    [SerializeField] private bool canDamage = true;

    [SerializeField] private int maxDashCount = 1;
    private int dashPhase = 0;
    
    [SerializeField] private AudioClip _dashClip;
    private AudioSource _audioSource;

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
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!inputHandler.IsInputActive()) return;
        
        desiredDash = inputHandler.InputSource.GetDashInput();

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
            
            dashDirection = GetDashDirection();
            
            EndDash();
        }

        if (isDashing)
        {
            PerformDash();
        }
    }

    private Vector2 GetDashDirection()
    {
        return CrossHair.GetCrossHairPosition() - transform.position;
    }
    
    private void PerformDash()
    {
        trailRenderer.emitting = true;
        StartCoroutine(DamageFrames(damageFrames));
        rigidbody2D.velocity = dashDirection.normalized * dashVelocity;
    }

    private void EndDash()
    {
        StartCoroutine(StopDashing());
        StartCoroutine(ClearDashTrail());
    }
    
    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashTime);
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
    
    private IEnumerator ClearDashTrail()
    {
        yield return new WaitForSeconds(dashTrailTime);
        trailRenderer.emitting = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(!col.gameObject.CompareTag("Enemy")) return;

        if (canDamage)
        {
            var enemyActor = col.gameObject.GetComponent<Enemy>();
            enemyActor.Damage(dashDamage);
        }

        EndDash();
    }
}