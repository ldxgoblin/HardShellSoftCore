using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    private static Vector3 mousePosition;
    
    private void Update()
    {
        transform.position = GetCrossHairPosition();
    }

    public static Vector3 GetCrossHairPosition()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 5.23f;

        Vector3 crossHairPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        return crossHairPosition;
    }
}
