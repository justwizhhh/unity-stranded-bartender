using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //*---------------------------------------------*
    //
    //  A global script for tweaking player physics and for other objects to retreive info on other components more easily
    //
    //*---------------------------------------------*

    [Header("Player Movement")]
    public LayerMask GroundLayer;
    public float Gravity;
    public float GravityAccel;
    [Space(10)]
    public float WalkSpeed;
    public float RunSpeed;
    public float Acceleration;
    [Space(10)]
    public float JumpHeight;
    public float JumpTimer;
    [Space(10)]
    public float CameraRotateSpeed;

    [Header("Components")]
    public Player player;
    public PlayerStateMachine stateMachine;

    public CapsuleCollider col;
    public CharacterController charCon;
    public MeshRenderer mr;

    public Camera cam;
    public CinemachineVirtualCamera virtualCam;
    public CinemachineVirtualCamera virtualCamNoDamp;
    public CameraController camCon;

    public LevelManager lm;

    private void Awake()
    {
        // Initialising all components
        player = GetComponent<Player>();
        stateMachine = GetComponent<PlayerStateMachine>();

        col = GetComponent<CapsuleCollider>();
        charCon = GetComponent<CharacterController>();
        mr = transform.GetChild(0).GetComponent<MeshRenderer>();

        cam = FindObjectOfType<Camera>();
        virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
        virtualCamNoDamp = virtualCam.GetComponentInChildren<CinemachineVirtualCamera>();
        camCon = cam.GetComponent<CameraController>();

        lm = FindAnyObjectByType<LevelManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
