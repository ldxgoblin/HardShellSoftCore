using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float boostHeight = 4f;
    [SerializeField, Range(0, 200)] private float maxBoostFuel = 2;
    [SerializeField, Range(0.25f, 1f)] private float fuelDepletionRate = 0.25f;
    [SerializeField, Range(0f, 5f)] private float downwardMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMultiplier = 3f;

    private InputHandler inputHandler;
    private InputSource inputSource = null;
    
    private Rigidbody2D rigidbody2D;
    private GroundCheck groundCheck;
    private Vector2 velocity;

    [SerializeField] private float boostFuelUsed;
    private float defaultGravityScale = 1f;

    private bool boosterRequested;
    private bool isBoosting;
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
        
        boosterRequested = inputHandler.InputSource.GetBoosterInput();
        
        velocity = rigidbody2D.velocity;

        isOnGround = groundCheck.GetCurrentGroundState();
        // reset boost counter
        if (isOnGround)
        {
            boostFuelUsed = 0;
        }
        
        // check if a boost was requested and perform it
        if (boosterRequested)
        {
            if (boostFuelUsed < maxBoostFuel)
            {
                isBoosting = true;
                PerformBoost();
            }
            else
            {
                isBoosting = false;
            }
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

    private void FixedUpdate()
    {
        if (isBoosting)
        {
            PerformBoost();
        }
    }

    private void PerformBoost()
    {
        boostFuelUsed += 1 * fuelDepletionRate;
        float boostSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * boostHeight);
            
        if (velocity.y >= 0)
        {
            boostSpeed = Mathf.Max(boostSpeed - velocity.y, 0f);
        }

        velocity.y += boostSpeed;
    }

}
