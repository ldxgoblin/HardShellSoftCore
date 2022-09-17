using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class MouseAimAndShoot : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private float delayBetweenShots;

    [SerializeField] private float projectileSpread = 1f;
    [SerializeField] private VisualEffect muzzleFlash;
    [SerializeField] private SimpleAudioEvent shotAudioEvent;

    public bool canFire;
    private readonly float impulseModifier = 0.025f;

    private Vector3 aimTarget;
    private AudioSource audioSource;
    private InputSource inputSource = null;

    private Camera mainCamera;
    private Vector3 mousePosition;

    private float shotCooldown;
    public static event Action OnPlayerShotFired;
    public static event Action<bool> OnLookDirectionChange;
    
    private CinemachineImpulseSource shotImpulseSource;

    private void Awake()
    {
        mainCamera = Camera.main;

        audioSource = GetComponent<AudioSource>();
        shotImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (!inputHandler.IsInputActive()) return;

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

        if (inputHandler.InputSource.GetFireInput() && canFire)
        {
            canFire = false;

            ShootAtMousePosition(transform.right);
            muzzleFlash.Play();

            shotImpulseSource.GenerateImpulse(-mousePosition * impulseModifier);

            shotAudioEvent.Play(audioSource);
        }
    }
    
    private void ShootAtMousePosition(Vector3 direction)
    {
        OnPlayerShotFired?.Invoke();
        
        var newProjectile = Instantiate(projectile, projectileTransform.position, Quaternion.identity);

        var spread = new Vector3(0, Random.Range(-projectileSpread, projectileSpread), 0);
        newProjectile.GetComponent<PlayerProjectile>().SetupProjectile(direction + spread);
    }

    private void AimAtMousePosition()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 5.23f;

        var targetPosition = mainCamera.WorldToScreenPoint(transform.position);
        mousePosition.x -= targetPosition.x;
        mousePosition.y -= targetPosition.y;

        var angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(0, 0, angle);

        var aimLocalScale = Vector3.one;

        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
            OnLookDirectionChange?.Invoke(true);
        }
        else
        {
            aimLocalScale.y = 1f;
            OnLookDirectionChange?.Invoke(false);
        }

        transform.localScale = aimLocalScale;
    }
}