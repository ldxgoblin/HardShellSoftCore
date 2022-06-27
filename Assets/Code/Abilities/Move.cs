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
        inputHandler = GetComponent<InputHandler>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundCheck = GetComponent<GroundCheck>();

        animator = GetComponent<Animator>();

        facingLeft = new Vector2(-transform.localScale.x, transform.localScale.y);
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
            FlipSprite();
        }
        
        if (direction.x < 0 && !isFacingLeft)
        {
            isFacingLeft = true;
            FlipSprite();
        }
        
        rigidbody2D.velocity = velocity;
        
        //animator.SetFloat("Speed", velocity.x);
    }
    
    // this whole flipping business should be extracted into a character class as it is only barely related to movement
    private void FlipSprite()
    {
        if (isFacingLeft)
        {
            // moving left
            //transform.localScale = facingLeft;
            spriteRenderer.flipX = true;
        } 
        else
        {
            // moving right
            //transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            spriteRenderer.flipX = false;
        }
    }
}
