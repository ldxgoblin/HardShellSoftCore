using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private float friction;
    private bool isOnGround;

    private void OnCollisionEnter2D(Collision2D other)
    {
        CheckForFlatSurface(other);
        GetFriction(other);
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isOnGround = false;
        friction = 0;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        CheckForFlatSurface(other);
        GetFriction(other);
    }

    private void CheckForFlatSurface(Collision2D other)
    {
        for (var i = 0; i < other.contactCount; i++)
        {
            var normal = other.GetContact(i).normal;
            // a normal greater than 0.9f means we are colliding with a flat surface, this needs to be extended for slopes
            isOnGround = normal.y >= 0.9f;
        }
    }

    private void GetFriction(Collision2D other)
    {
        var material = other.collider.sharedMaterial;
        friction = 0;

        if (material != null) friction = material.friction;
    }

    public bool GetCurrentGroundState()
    {
        return isOnGround;
    }

    public float GetFriction()
    {
        return friction;
    }
}