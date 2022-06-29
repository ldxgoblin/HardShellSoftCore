using System;
using UnityEngine;

[RequireComponent(typeof(GroundCheck),typeof(InputHandler))]
public class Move : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;
    private GroundCheck groundCheck;

    private float maxSpeedChange;
    private float acceleration;
    private bool isOnGround;

    private InputHandler inputHandler;

    private Vector2 facingLeft;
    public bool isFacingLeft;

    private Animator animator;
    
    private void Awake()
    {
        MouseAimAndShoot.onLookDirectionChange += FlipSpriteX;
        
        inputHandler = GetComponent<InputHandler>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        groundCheck = GetComponent<GroundCheck>();

        animator = GetComponent<Animator>();

        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    private void OnDestroy()
    {
        MouseAimAndShoot.onLookDirectionChange -= FlipSpriteX;
    }

    private void Update()
    {
        if(!inputHandler.IsInputActive()) return;
        
        direction.x = inputHandler.InputSource.GetHorizontalInput();
        
        // this way we always have a velocity value above 0
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - groundCheck.GetFriction(), 0f);
    }

    private void FixedUpdate()
    {
        if(!inputHandler.IsInputActive()) return;
        
        isOnGround = groundCheck.GetCurrentGroundState();
        velocity = rigidbody2D.velocity;

        acceleration = isOnGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        if (direction.x > 0 && isFacingLeft)
        {
            isFacingLeft = false;
        }
        
        if (direction.x < 0 && !isFacingLeft)
        {
            isFacingLeft = true;
        }

        FlipSpriteX(isFacingLeft);
        rigidbody2D.velocity = velocity;
        
        //animator.SetFloat("Speed", velocity.x);
    }
    
    private void FlipSpriteX(bool flipX)
    {
        spriteRenderer.flipX = flipX;
    }
}
