using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float horizontalMovement = 0f;

    private bool jump = false;
    [SerializeField] private bool crouch = false;
    
    [Header("Movement Setup")] [Space]
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private float timeZeroToMax = 0.5f;
    [SerializeField] private float timeMaxToZero = 6f;

    private float forwardVelocity;
    
    private float accelRatePerSec;
    private float decelRatePerSec;

    private void Awake()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;

        forwardVelocity = 0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        
        if (characterController == null)
        {
            Debug.LogError("Player Character Controller missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        
        if (horizontalMovement != 0) 
        {
            Accelerate(accelRatePerSec);
        }
        else
        {
            Accelerate(decelRatePerSec);
        }
        
        horizontalMovement *= forwardVelocity;
        
        HandleInput();
    }

    private void Accelerate(float accel)
    {
        forwardVelocity += accel * Time.deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, 0, maxSpeed);
    }
    
    private void HandleInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    void FixedUpdate()
    {
        characterController.Move(horizontalMovement * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
    
}
