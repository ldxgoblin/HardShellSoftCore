using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementModule : MonoBehaviour
{

    [SerializeField] private float _speed = 1000f;
    [SerializeField] private float _jumpStrength = 15f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _jumpButtonPressed;

    private float _horizontalInput;
    
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        HandleMovement();
        HandleJumping();
    }
    
    void FixedUpdate()
    {
        if (_jumpButtonPressed)
        {
            _jumpButtonPressed = false;
            _rigidbody.AddForce(Vector2.up * _jumpStrength, ForceMode.Impulse);
        }
    }

    private void HandleCharacterFlip()
    {
        var scale = transform.localScale;

        if (_horizontalInput < 0)
        {
            // we want to move left so flip x scale
            scale.x = -1.2f;
        }

        if (_horizontalInput > 0)
        {
            scale.x = 1.2f;
        }

        transform.localScale = scale;
    }
    private void HandleJumping()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpButtonPressed = true;
        }
    }

    private void HandleMovement()
    {
        _rigidbody.AddForce(Vector2.right * _horizontalInput * _speed * Time.deltaTime);
    }
    
    private float Abs(float value)
    {
        if (value >= 0f) return value;

        return -value;
    }
}