using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] protected int projectileDamage;
    [SerializeField] protected float projectileSpeed;
    [SerializeField] protected int destroyAfterSeconds = 2;
    protected Vector3 direction;

    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public virtual void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }

    public void SetupProjectile(Vector3 direction)
    {
        this.direction = direction;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFloatFromVector3(direction));

        Vector2 velocity = direction * projectileSpeed;
        rigidbody2D.velocity = velocity;

        AutoDestruct();
    }


    protected virtual void AutoDestruct()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    private float GetAngleFloatFromVector3(Vector3 direction)
    {
        direction = direction.normalized;
        var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

        if (angle < 0) angle += 360;

        return angle;
    }
}