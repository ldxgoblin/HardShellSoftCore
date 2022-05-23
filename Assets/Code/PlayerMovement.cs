using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float horizontalMovement = 0f;

    private bool jump = false;
    [SerializeField] private bool crouch = false;
    
    [Header("Movement Setup")] [Space]
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private float timeZeroToMax = 0.5f;
    [SerializeField] private float timeMaxToZero = 6f;
    [SerializeField] private float timeBrakeToZero = 1f;
    
    private float forwardVelocity;
    
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float brakeRatePerSec;
    
    private void Awake()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
        brakeRatePerSec = -maxSpeed / timeBrakeToZero;
        
        forwardVelocity = 0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        forwardVelocity += accelRatePerSec * Time.deltaTime;
        forwardVelocity = Mathf.Min(forwardVelocity, maxSpeed);
        
        horizontalMovement = Input.GetAxisRaw("Horizontal") * forwardVelocity;
        
        HandleInput();
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
        _characterController.Move(horizontalMovement * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
    
}
