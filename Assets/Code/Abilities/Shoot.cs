using Cinemachine;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    private InputSource inputSource = null;
    
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileTransform;
    [SerializeField] private float delayBetweenShots;
    [SerializeField] private float aimRotationSpeed = 5f;
    [SerializeField] private AudioClip shotClip;
    
    private Camera mainCamera;

    private CinemachineImpulseSource shotImpulseSource;
    private float impulseModifier = 0.025f;
    private AudioSource shotAudioSource;
    private float shotCooldown;
    public bool canFire;
    
    private Vector3 aimDirection;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        
        shotAudioSource = GetComponent<AudioSource>();
        shotImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        // if(!inputHandler.IsInputActive()) return;

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

            Vector3 direction = transform.position - aimDirection;;
            shotImpulseSource.GenerateImpulse(-direction * impulseModifier);
            
            shotAudioSource.PlayOneShot(shotClip);
        }
    }
    
    private void RotateTowardsMousePosition()
    {
        aimDirection = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.z));
        
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //Quaternion aimRotation = Quaternion.AngleAxis(aimAngle, Vector3.forward);
        Quaternion aimRotation = Quaternion.LookRotation(Vector3.forward, aimDirection);
        Debug.Log($"{aimRotation.eulerAngles}");
        
        transform.rotation = Quaternion.Slerp(transform.rotation, aimRotation, aimRotationSpeed * Time.deltaTime);
    }
}
