using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    
    Vector3 originalCameraPosition;
    float shakeAmt = 0;
    public Camera mainCamera;

    void OnCollisionEnter2D(Collision2D coll) 
    {
        shakeAmt = coll.relativeVelocity.magnitude * .0025f;
        InvokeRepeating("ShakeCamera", 0, .01f);
        Invoke("StopShaking", 0.3f);
    }

    void ShakeCamera()
    {
        if(shakeAmt>0) 
        {
            float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
            Vector3 pp = mainCamera.transform.position;
            pp.y+= quakeAmt; // can also add to x and/or z
            mainCamera.transform.position = pp;
        }
    }

    void StopShaking()
    {
        CancelInvoke("ShakeCamera");
        mainCamera.transform.position = originalCameraPosition;
    }

    
}