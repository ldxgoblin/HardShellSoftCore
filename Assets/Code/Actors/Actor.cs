using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] protected HitPoints hitPoints;
    [SerializeField] protected Material hitMaterial;

    protected AudioSource audioSource;

    private Color baseSpriteColor;
    private Material baseSpriteMaterial;

    protected bool isInvincible;
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
        if (!isInvincible)
        {
            if (hitPoints.currentHitPoints - damage <= 0)
            {
                hitPoints.currentHitPoints = 0;
                Die();
            }
            else
            {
                hitPoints.DecreaseHitPoints(damage);
                StartCoroutine(HitFlash());
            }
        }
    }

    protected virtual void Die()
    {
        // TODO: Object Pooling
        gameObject.SetActive(false);
    }

    protected IEnumerator HitFlash()
    {
        // TODO: sometimes causes a nullref when this method returns to
        // the main thread and the associated gameobject is inactive

        spriteRenderer.color = Color.white;
        spriteRenderer.material = hitMaterial;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = baseSpriteColor;
        spriteRenderer.material = baseSpriteMaterial;
    }
}