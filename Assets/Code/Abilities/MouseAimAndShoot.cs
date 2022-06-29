using System;
using Cinemachine;
using UnityEngine;

public class MouseAimAndShoot : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    private InputSource inputSource = null;
    
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private float delayBetweenShots;
    [SerializeField] private AudioClip shotClip;
    
    private Camera mainCamera;

    private CinemachineImpulseSource shotImpulseSource;
    private float impulseModifier = 0.025f;
    private AudioSource shotAudioSource;
    private float shotCooldown;
    
    public bool canFire;
    
    private Vector3 aimTarget;
    private Vector3 mousePosition;

    public static event Action<bool> onLookDirectionChange;

    private void Awake()
    {
        mainCamera = Camera.main;
        
        shotAudioSource = GetComponent<AudioSource>();
        shotImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if(!inputHandler.IsInputActive()) return;

        AimAtMousePosition();
        
        if (!canFire)
        {
            shotCooldown += Time.deltaTime;
            if (shotCooldown > delayBetweenShots)
            {
                canFire = true;
                shotCooldown = 0;
            }
        }
        
        if(inputHandler.InputSource.GetFireInput() && canFire)
        {
            canFire = false;

            ShootAtMousePosition(transform.right);
            
            shotImpulseSource.GenerateImpulse(-mousePosition * impulseModifier);
            shotAudioSource.PlayOneShot(shotClip);
        }
        
    }

    private void ShootAtMousePosition(Vector3 direction)
    {
        var newProjectile = Instantiate(projectile, projectileTransform.position, Quaternion.identity);
        newProjectile.GetComponent<BasicProjectile>().SetupProjectile(direction);
    }
    
    private void AimAtMousePosition()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 5.23f;

        Vector3 targetPosition = mainCamera.WorldToScreenPoint(transform.position);
        mousePosition.x -= targetPosition.x;
        mousePosition.y -= targetPosition.y;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        
        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
            onLookDirectionChange?.Invoke(true);
        }
        else
        {
            aimLocalScale.y = 1f;
            onLookDirectionChange?.Invoke(false);
        }
        
        transform.localScale = aimLocalScale;
    }
}