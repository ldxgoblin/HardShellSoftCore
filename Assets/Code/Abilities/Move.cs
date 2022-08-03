using UnityEngine;

[RequireComponent(typeof(GroundCheck), typeof(InputHandler))]
public class Move : MonoBehaviour
{
    [SerializeField] [Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField] [Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField] [Range(0f, 100f)] private float maxAirAcceleration = 20f;
    public bool isFacingLeft;

    [SerializeField] private bool isMoving;
    private float acceleration;

    private Animator animator;
    private Vector2 desiredVelocity;

    private Vector2 direction;

    private Vector2 facingLeft;
    private GroundCheck groundCheck;

    private InputHandler inputHandler;
    private bool isOnGround;

    private float maxSpeedChange;
    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;
    private Vector2 velocity;

    private void Awake()
    {
        MouseAimAndShoot.OnLookDirectionChange += FlipSpriteX;
        Dash.OnLookDirectionChange += FlipSpriteX;

        inputHandler = GetComponent<InputHandler>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        groundCheck = GetComponent<GroundCheck>();

        animator = GetComponent<Animator>();

        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    private void Update()
    {
        if (!inputHandler.IsInputActive()) return;

        direction.x = inputHandler.InputSource.GetHorizontalInput();

        if (direction.x == 0) isMoving = false;
        else isMoving = true;

        // this way we always have a velocity value above 0
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - groundCheck.GetFriction(), 0f);
    }

    private void FixedUpdate()
    {
        if (!inputHandler.IsInputActive()) return;

        isOnGround = groundCheck.GetCurrentGroundState();
        velocity = rigidbody2D.velocity;

        acceleration = isOnGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        if (direction.x > 0 && isFacingLeft) isFacingLeft = false;

        if (direction.x < 0 && !isFacingLeft) isFacingLeft = true;

        if (isMoving) FlipSpriteX(isFacingLeft);
        rigidbody2D.velocity = velocity;

        //animator.SetFloat("Speed", velocity.x);
    }

    private void OnDestroy()
    {
        MouseAimAndShoot.OnLookDirectionChange -= FlipSpriteX;
        Dash.OnLookDirectionChange -= FlipSpriteX;
    }

    private void FlipSpriteX(bool flipX)
    {
        spriteRenderer.flipX = flipX;
    }
}