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


    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        baseSpriteColor = spriteRenderer.color;

        baseSpriteMaterial = spriteRenderer.material;
        rigidbody2D = GetComponent<Rigidbody2D>();

        hitPoints.ResetHitPoints();
    }

    public void Damage(int damage)
    {
        if (hitPoints.currentHitPoints - damage <= 0)
        {
            Die();
        }
        else
        {
            hitPoints.DecreaseHitPoints(damage);
            HitFlash();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    private async void HitFlash()
    {
        if (spriteRenderer == null) return;

        spriteRenderer.color = Color.white;
        spriteRenderer.material = hitMaterial;
        await Task.Delay(100);
        spriteRenderer.color = baseSpriteColor;
        spriteRenderer.material = baseSpriteMaterial;
    }
}