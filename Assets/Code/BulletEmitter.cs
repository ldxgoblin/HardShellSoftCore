using UnityEngine;

public class BulletEmitter : MonoBehaviour
{
    [Header ("Bullet Characteristics")]
    [Tooltip("This is how the bullets look.")][SerializeField] private Sprite bulletSprite;
    [Tooltip("Make it pretty.")][SerializeField] private Color bulletBaseColor = Color.white;
    
    [Tooltip("The higher the value, the more it looks like a ring :)!")][Range(1,359)][SerializeField] private int bulletSpreadAngle = 1;
    [Tooltip("This is how fast the bullets travel.")][Range(1f,10f)][SerializeField] private float bulletSpeed = 3f;
    
    [Tooltip("The amount of Seconds before the bullet is destroyed.")][Range(0.25f,5f)][SerializeField] private float bulletLifeTime = 5f;
    [Tooltip("The lower the value, the higher the shot frequency!")][Range(0.1f,1f)][SerializeField] private float delayBetweenBullets = 0.5f;
    [Tooltip("Size matters!")][Range(0.1f,1f)][SerializeField] private float bulletSize = 1f;
    
    [SerializeField] private float startSpawningAfterSeconds = 1f;
    private float angle;
    
    private ParticleSystem bulletParticleSystem;
    
    [SerializeField] private Material material;
    [SerializeField] private GameObject bulletTemplate;
    
    private void Awake()
    {
        // use template if given, else create after inspector specifications
        if(bulletTemplate == null) CreateBasicBulletSpawners();
        else CreateBulletSpawnersFromTemplate(bulletTemplate.GetComponent<ParticleSystem>());
    }

    /// <summary>
    /// Creates and rotates a Particle System for every given Angle in Inspector
    /// Example: bulletSpreadAngle set to 16 means 16 Particle Systems are created
    /// at the same position and each one is given an angle and will shoot straight by default
    /// </summary>
    private void CreateBasicBulletSpawners()
    {
        angle = 360f / bulletSpreadAngle;
        
        for (int i = 0; i < bulletSpreadAngle; i++)
        {
            Material bulletMaterial = material;

            // Create a bullet Particle System.
            var bulletSpawner = new GameObject("Bullet Spawner");
            
            bulletSpawner.transform.Rotate(angle * i, 90, 44);
            bulletSpawner.transform.parent = transform;
            bulletSpawner.transform.position = transform.position;
            
            bulletParticleSystem = bulletSpawner.AddComponent<ParticleSystem>();
            bulletSpawner.GetComponent<ParticleSystemRenderer>().material = bulletMaterial;
           
            var mainModule = bulletParticleSystem.main;
            mainModule.startColor = bulletBaseColor;
            mainModule.startSize = bulletSize;
            mainModule.startSpeed = bulletSpeed;
            mainModule.maxParticles = 100000;

            var emission = bulletParticleSystem.emission;
            emission.enabled = false;

            var shape = bulletParticleSystem.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Sprite;
            shape.sprite = null;
        }

        InvokeRepeating("SpawnBullets", startSpawningAfterSeconds, delayBetweenBullets);
    }

    private void CreateBulletSpawnersFromTemplate(ParticleSystem template)
    {
        angle = 360f / bulletSpreadAngle;
        
        for (int i = 0; i < bulletSpreadAngle; i++)
        {
            // Create a bullet Particle System from given Template for every given angle.
            var bulletSpawner = Instantiate(bulletTemplate, transform.position, Quaternion.identity, transform);
            bulletSpawner.transform.Rotate(angle * i, 90, 0);
        }

        InvokeRepeating("SpawnBulletsFromTemplate", startSpawningAfterSeconds, delayBetweenBullets);
    }
    
    void SpawnBullets()
    {
        foreach (Transform child in transform)
        {
            bulletParticleSystem = child.GetComponent<ParticleSystem>();
            // Any parameters we assign in emitParams will override the current system's when we call Emit.
            // Here we will override the start color and size.
            var emitParams = new ParticleSystem.EmitParams();
            emitParams.startColor = bulletBaseColor;
            emitParams.startSize = bulletSize;
            emitParams.startLifetime = bulletLifeTime;
            bulletParticleSystem.Emit(emitParams, 1000);
        }
    }
    
    void SpawnBulletsFromTemplate()
    {
        foreach (Transform child in transform)
        {
            bulletParticleSystem = child.GetComponent<ParticleSystem>();
            bulletParticleSystem.Emit(new ParticleSystem.EmitParams(), 1);
        }
    }
}
