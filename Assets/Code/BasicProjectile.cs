using System;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private int projectileDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private bool isPhysical;

    [SerializeField] private int destroyAfterSeconds = 2;
    
    private Vector3 direction;
    
    
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
        transform.position += direction * projectileSpeed * Time.deltaTime;
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
