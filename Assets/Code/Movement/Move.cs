using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private InputController inputSource = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D rigidbody2D;
    private GroundCheck groundCheck;

    private float maxSpeedChange;
    private float acceleration;
    private bool isOnGround;
    
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<GroundCheck>();
    }
    
    private void Update()
    {
        direction.x = inputSource.GetMovementInput();
        // this way we always have a velocity value above 0
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - groundCheck.GetFriction(), 0f);
    }

    private void FixedUpdate()
    {
        isOnGround = groundCheck.GetCurrentGroundState();
        velocity = rigidbody2D.velocity;

        acceleration = isOnGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;

        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        rigidbody2D.velocity = velocity;
    }
}
