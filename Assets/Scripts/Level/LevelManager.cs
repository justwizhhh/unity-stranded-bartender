using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LevelManager : MonoBehaviour
{
    //*---------------------------------------------*
    //
    //  A dedicated object for level settings and for retrieving info about all of the objects in the level
    //
    //*---------------------------------------------*

    [Header("Debug")]
    public bool DebugVisuals;

    // Object references
    Player player;
    Camera cam;

    Light[] lightDebug;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        cam = Camera.main;

        lightDebug = FindObjectsOfType<Light>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (DebugVisuals)
        {
            foreach (Light light in lightDebug)
            {
                light.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
