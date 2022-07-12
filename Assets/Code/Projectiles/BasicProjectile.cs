using System;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] protected int projectileDamage;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected int destroyAfterSeconds = 2;

    private Rigidbody2D rigidbody2D;
    protected Vector3 direction;

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

    private void FixedUpdate()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        Vector2 velocity = direction * projectileSpeed;
        rigidbody2D.MovePosition(rigidbody2D.position + velocity * Time.fixedDeltaTime);
    }

    private void AutoDestruct()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"{this.gameObject.name}'s bullet hit: {col.gameObject.name} !");
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
