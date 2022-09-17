using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(InputHandler))]
public class Dash : MonoBehaviour
{
    [SerializeField] [Range(0.05f, 0.1f)] private float dashTime = 0.1f;
    [SerializeField] private float dashRate = 1f;
    [SerializeField] private float dashSpeed = 450f;
    [SerializeField] [Range(1, 10)] private int dashDamage = 1;
    [SerializeField] private int invincibilityFrames = 60;

    [SerializeField] private GameObject dashImpactFX;

    [SerializeField] private SimpleAudioEvent dashAudioEvent;
    [SerializeField] private SimpleAudioEvent dashHitAudioEvent;

    [SerializeField] private SlowMotion slowMotion;
    [SerializeField] private float slowMotionDuration = 0.5f;

    [SerializeField] private bool canDamage;
    [SerializeField] private SpriteRenderer playerSprite;

    private AudioSource audioSource;
    private bool canDash = true;
    private CircleCollider2D collider2D;
    private float dashCoolDown;

    private Vector2 dashDirection;

    private readonly float dashTrailTime = 0.35f;

    private bool desiredDash;

    private InputHandler inputHandler;
    private InputSource inputSource;

    private bool isDashing;
    private bool isOnGround;
    private Player player;

    private Rigidbody2D rigidbody2D;

    private TrailRenderer trailRenderer;
    
    public static event Action OnDashHit;
    public static event Action OnDash;
    public static event Action<int> OnDashDamage;
    public static event Action<bool> OnLookDirectionChange;

    private void Awake()
    {
        dashCoolDown = 0;

        inputHandler = GetComponent<InputHandler>();
        if (inputHandler.InputSource != null) inputSource = inputHandler.InputSource;

        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<CircleCollider2D>();

        player = GetComponent<Player>();
        trailRenderer = GetComponent<TrailRenderer>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!inputHandler.IsInputActive()) return;

        desiredDash = inputHandler.InputSource.GetDashInput();

        if (desiredDash)
        {
            dashDirection = GetDashDirection();
            StartCoroutine(GrantDamageAndIFrames(invincibilityFrames));
        }

        if (desiredDash && canDash)
        {
            if (dashCoolDown <= 0)
            {
                rigidbody2D.velocity = Vector2.zero;

                isDashing = true;
                canDash = false;
                canDamage = true;

                dashAudioEvent.Play(audioSource);
                
                OnDash?.Invoke();

                ResetDashCoolDown();
            }

            EndDash();
        }

        if (dashCoolDown > 0) dashCoolDown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (isDashing) PerformDash();
    }

    private void OnEnable()
    {
        ResetDashCoolDown();
        trailRenderer.emitting = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (canDamage)
            {
                var enemyActor = col.gameObject.GetComponent<Actor>();

                enemyActor.Damage(dashDamage);
                
                Instantiate(dashImpactFX, transform.position, Quaternion.identity);

                OnDashHit?.Invoke();
                
                OnDash?.Invoke();
                OnDashDamage?.Invoke(1);
                
                dashHitAudioEvent.Play(audioSource);
                slowMotion.SlowDown(slowMotionDuration, 0.15f);
            }

            StopDashInstantly();
        }
    }
    
    private void ResetDashCoolDown()
    {
        dashCoolDown = dashRate;
    }

    private Vector2 GetDashDirection()
    {
        Vector2 crossHairPosition = CrossHair.GetCrossHairPosition() - transform.position;
        LookAtTarget(crossHairPosition);
        return crossHairPosition.normalized;
    }

    private void PerformDash()
    {
        trailRenderer.emitting = true;

        var force = dashDirection * (dashSpeed * Time.deltaTime);
        rigidbody2D.AddForce(force, ForceMode2D.Impulse);

        //StartCoroutine(GrantDamageAndIFrames(invincibilityFrames));
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

    public void StopDashInstantly()
    {
        // failsafe
        isDashing = false;
        canDash = true;
        player.SetInvincibility(false);
    }

    public void StopDamaging()
    {
        canDamage = false;
    }
    
    private IEnumerator GrantDamageAndIFrames(int frames)
    {
        player.SetInvincibility(true);
        canDamage = true;

        for (var i = 0; i < frames; i++) yield return new WaitForEndOfFrame();

        player.SetInvincibility(false);
        StopDamaging();
    }

    private IEnumerator ClearDashTrailDelayed()
    {
        yield return new WaitForSeconds(dashTrailTime);
        ClearDashTrailInstantly();
    }

    private void ClearDashTrailInstantly()
    {
        trailRenderer.emitting = false;
    }

    private void LookAtTarget(Vector2 lookdir)
    {
        var lookDirection = lookdir;

        if (lookDirection.x > 0)
            OnLookDirectionChange?.Invoke(false);
        else if (lookDirection.x < 0) OnLookDirectionChange?.Invoke(true);
    }
}