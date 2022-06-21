using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)] private float shakeAmount;
    [SerializeField] private Camera mainCamera;
    private Vector3 originalCameraPosition;
    
    private void OnCollisionEnter2D(Collision2D coll) 
    {
        shakeAmount = coll.relativeVelocity.magnitude * .0025f;
        InvokeRepeating("ShakeCamera", 0, .01f);
        Invoke("StopShaking", 0.3f);
    }

    private void ShakeCamera()
    {
        if(shakeAmount>0) 
        {
            float shakeStrength = Random.value*shakeAmount*2 - shakeAmount;
            Vector3 pp = mainCamera.transform.position;
            pp.y+= shakeStrength; // can also add to x and/or z
            mainCamera.transform.position = pp;
        }
    }

    private void StopShaking()
    {
        CancelInvoke("ShakeCamera");
        mainCamera.transform.position = originalCameraPosition;
    }
}