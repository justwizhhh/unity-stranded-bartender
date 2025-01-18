using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    //*---------------------------------------------*
    //
    //  The player's main camera moving code
    //
    //*---------------------------------------------*

    // Camera Position States
    public GameObject[] StateCameras;


    // Object references
    Camera cam;
    CinemachineBrain camBrain;

    private void Awake()
    {
        cam = Camera.main;
        camBrain = GetComponent<CinemachineBrain>();
    }

    public void ChangeAnchor(int cam_id)
    {
        for (int i = 0; i < StateCameras.Length; i++)
        {
            if (i == cam_id)
            {
                StateCameras[i].SetActive(true);
            }
            else
            {
                StateCameras[i].SetActive(false);
            }
        }
    }

    // Set the transition speed between virtual cameras
    public void ChangeSpeed(float speed)
    {
        camBrain.m_DefaultBlend.m_Time = speed;
    }
}
