using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHitFx : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("EnemyProjectile")) Destroy(col.gameObject);
    }
}
