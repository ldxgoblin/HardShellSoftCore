using UnityEngine;

[RequireComponent(typeof(GroundCheck),typeof(InputHandler))]
public class Jump : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float jumpHeight = 4f;
    [SerializeField, Range(0, 5)] private int maxJumpCount = 2;
    [SerializeField, Range(0f, 5f)] private float downwardMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMultiplier = 3f;

    private InputHandler inputHandler;
    private InputSource inputSource = null;
    
    private Rigidbody2D rigidbody2D;
    private GroundCheck groundCheck;
    private Vector2 velocity;

    private int jumpPhase;
    private float defaultGravityScale = 1f;

    private bool desiredJump;
    private bool isOnGround;
    
    [SerializeField] private AudioClip _jumpClip;
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

        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!inputHandler.IsInputActive()) return;
        
        // using the OR Operator this value remains set until we change it to false manually
        desiredJump |= inputHandler.InputSource.GetJumpInput();
    }

    private void FixedUpdate()
    {
        if(!inputHandler.IsInputActive()) return;
        
        isOnGround = groundCheck.GetCurrentGroundState();
        velocity = rigidbody2D.velocity;
        
        // reset jump counter
        if (isOnGround)
        {
            jumpPhase = 0;
        }
        
        // check if a jump was requested and perform it
        if (desiredJump)
        {
            desiredJump = false;
            PerformJump();
        }
        
        // multipliers
        if (rigidbody2D.velocity.y > 0)
        {
            // we're going up
            rigidbody2D.gravityScale = upwardMultiplier;
        }
        else if (rigidbody2D.velocity.y < 0)
        {
            // we're going down
            rigidbody2D.gravityScale = downwardMultiplier;
        }
        else if (rigidbody2D.velocity.y == 0)
        {
            // we're on the ground
            rigidbody2D.gravityScale = defaultGravityScale;
        }

        rigidbody2D.velocity = velocity;
    }

    private void PerformJump()
    {
        if (isOnGround || jumpPhase < maxJumpCount)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            
            if (velocity.y > 0)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }

            velocity.y += jumpSpeed;
        }
    }
}
