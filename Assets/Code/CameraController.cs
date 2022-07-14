using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerFollowCam;
    [SerializeField] private CinemachineVirtualCamera playerZoomCam;
    
    void Start()
    {
        SwitchToFollowCam();
    }

    private void SwitchToFollowCam()
    {
        playerZoomCam.Priority = 0;
        playerFollowCam.Priority = 10;
    }

    private void SwitchToZoomCam()
    {
        playerZoomCam.Priority = 10;
        playerFollowCam.Priority = 0;
    }
}
