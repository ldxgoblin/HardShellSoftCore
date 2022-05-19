using System;
using Cinemachine;
using UnityEngine;

public class ShootingModule : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Vector3 _mousePos;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _bulletTransform;

    [SerializeField] private AudioClip _shotClip;
    private AudioSource _shotSource;

    public bool canFire;
    private float _timer;
    [SerializeField] private float timeBetweenFiring;
    private CinemachineImpulseSource _shotImpulseSource;

    private float impulseModifier = 0.025f;

    private void Awake()
    {
        _shotSource = GetComponent<AudioSource>();
        _shotImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        _mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 rotation = _mousePos - transform.position;
        float zRotation = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        
        transform.rotation = Quaternion.Euler(0,0, zRotation);

        if (!canFire)
        {
            _timer += Time.deltaTime;
            if (_timer > timeBetweenFiring)
            {
                canFire = true;
                _timer = 0;
            }
        }
        
        if(Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            Instantiate(_bullet, _bulletTransform.position, Quaternion.identity);

            Vector3 direction = transform.position - _mousePos;
            _shotImpulseSource.GenerateImpulse(-direction * impulseModifier);
            
            _shotSource.PlayOneShot(_shotClip);
        }
    }
    
}
