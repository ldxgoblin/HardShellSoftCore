using System.Threading.Tasks;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] protected HitPoints hitPoints;

    [SerializeField] protected Material hitMaterial;

    protected Color baseSpriteColor;
    protected Material baseSpriteMaterial;
    protected Rigidbody2D rigidbody2D;

    protected SpriteRenderer spriteRenderer;
    public HitPoints HitPoints => hitPoints;


    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        baseSpriteColor = spriteRenderer.color;

        baseSpriteMaterial = spriteRenderer.material;
        rigidbody2D = GetComponent<Rigidbody2D>();

        hitPoints.ResetHitPoints();
    }

    public virtual void Damage(int damage)
    {
        if (hitPoints.currentHitPoints - damage <= 0)
        {
            hitPoints.currentHitPoints = 0;
            Die();
        }
        else
        {
            hitPoints.DecreaseHitPoints(damage);
            HitFlash();
        }
    }

    protected virtual void Die()
    {
        //Destroy(gameObject);
        
        // TODO: Object Pooling
        gameObject.SetActive(false);
    }

    protected virtual async void HitFlash()
    {
        // TODO: sometimes causes a nullref when this method returns to
        // the main thread and the associated gameobject is inactive

        spriteRenderer.color = Color.white;
        spriteRenderer.material = hitMaterial;
        await Task.Delay(100);
        spriteRenderer.color = baseSpriteColor;
        spriteRenderer.material = baseSpriteMaterial;
    }
}