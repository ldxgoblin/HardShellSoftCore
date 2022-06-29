using System;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private int projectileDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private int destroyAfterSeconds = 2;

    private Rigidbody2D rigidbody2D;
    private Vector3 direction;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void SetupProjectile(Vector3 direction)
    {
        this.direction = direction;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFloatFromVector3(direction));
        
        AutoDestruct();
    }

    private void Update()
    {
        MoveWithTransform();
    }

    private void MoveWithTransform()
    {
        //transform.position += direction * projectileSpeed * Time.deltaTime;
        rigidbody2D.position += (Vector2)direction * projectileSpeed * Time.deltaTime;

        //rigidbody2D.MovePosition();
        
        
        // To modify the position of the object with Rigidbody on it, always set Rigidbody.position when a 
        // new position doesn’t follow the previous one, or Rigidbody.MovePosition when it is a continuous movement, 
        // which also takes interpolation into account. When modifying it, apply operations always in FixedUpdate, 
        // not in Update functions. It will assure consistent physics behaviors.


    }

    private void MoveWithPhysics()
    {
        
    }

    private void AutoDestruct()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Bullet Hit!");
        Destroy(gameObject);
    }

    private float GetAngleFloatFromVector3(Vector3 direction)
    {
        direction = direction.normalized;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        
        if (angle < 0) angle += 360;

        return angle;
    }

}
