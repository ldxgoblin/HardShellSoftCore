using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Vector3 _mousePos;
    private Camera _mainCam;
    private Rigidbody _rigidbody;
    [SerializeField] private float _speed; 
    [SerializeField] private float _lifeTimeInSeconds;
    
    // Start is called before the first frame update
    void Start()
    {
        _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
        _rigidbody = GetComponent<Rigidbody>();
        _mousePos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 direction = _mousePos - transform.position;
        Vector3 rotation = transform.position - _mousePos;

        _rigidbody.velocity = new Vector3(direction.x, direction.y).normalized * _speed;

        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0, rot + 90);
        
        Destroy(gameObject, _lifeTimeInSeconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground")||other.CompareTag("Enemy")) Destroy(gameObject);
    }
}
