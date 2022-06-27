using System;
using Cinemachine;
using UnityEngine;

public class Shoot : MonoBehaviour
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
    
    // Debug Actions
    public static Action<Vector3, Vector3> onMousePositionUpdate;

    private void Awake()
    {
        mainCamera = Camera.main;
        
        shotAudioSource = GetComponent<AudioSource>();
        shotImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if(!inputHandler.IsInputActive()) return;

        RotateTowardsMousePosition();
        
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
            Instantiate(projectile, projectileTransform.position, Quaternion.identity);

            Vector3 direction = transform.position - aimTarget;;
            shotImpulseSource.GenerateImpulse(-direction * impulseModifier);
            
            shotAudioSource.PlayOneShot(shotClip);
        }
        
    }
    private void RotateTowardsMousePosition()
    {
        //this works only as long as we dont flip the localscale lol
        
        Vector3 mousePosition = Input.mousePosition;
        
        mousePosition.z = 5.23f;

        Vector3 targetPosition = mainCamera.WorldToScreenPoint(transform.position);
        mousePosition.x -= targetPosition.x;
        mousePosition.y -= targetPosition.y;

        float angle = 0f;
        
        angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        Debug.Log($"Current Z-Rotation: {angle}");
        
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}