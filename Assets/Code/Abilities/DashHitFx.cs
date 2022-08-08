using UnityEngine;

public class DashHitFx : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("EnemyProjectile")) Destroy(col.gameObject);
    }
}
