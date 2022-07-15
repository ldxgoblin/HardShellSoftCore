using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class MouseAimAndShoot : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private float delayBetweenShots;

    [SerializeField] private VisualEffect muzzleFlash;
    [SerializeField] private AudioClip shotClip;

    public bool canFire;
    private readonly float impulseModifier = 0.025f;

    private Vector3 aimTarget;
    private InputSource inputSource = null;

    private Camera mainCamera;
    private Vector3 mousePosition;
    private AudioSource shotAudioSource;
    private float shotCooldown;

    private CinemachineImpulseSource shotImpulseSource;

    private void Awake()
    {
        mainCamera = Camera.main;

        shotAudioSource = GetComponent<AudioSource>();
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
            shotAudioSource.PlayOneShot(shotClip);
        }
    }

    public static event Action<bool> onLookDirectionChange;

    private void ShootAtMousePosition(Vector3 direction)
    {
        var newProjectile = Instantiate(projectile, projectileTransform.position, Quaternion.identity);
        newProjectile.GetComponent<PlayerProjectile>().SetupProjectile(direction);
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