using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //*---------------------------------------------*
    //
    //  The player's main input/collision detection code
    //
    //*---------------------------------------------*

    // Inputs
    [HideInInspector] public Vector3 moveInput;
    [HideInInspector] public bool runInput;
    [HideInInspector] public bool jumpInput;
    [HideInInspector] public bool pickUpInput;
    [HideInInspector] public bool camRotInputLeft;
    [HideInInspector] public bool camRotInputRight;
    [HideInInspector] public bool quitInput;

    public InputActionMap inputs;

    // Player physics
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float moveAccel;
    [HideInInspector] public float currentGravity;
    [HideInInspector] public float currentJumpTimer;

    public Vector3 movingPlatformSpeed;

    public Transform PickUpAnchor;


    // Main components
    [HideInInspector] public PlayerController controller;
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        inputs.Enable();
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        velocity = new Vector3(
            controller.charCon.velocity.x,
            controller.stateMachine.CurrentState.GetType() == typeof(PlayerJump) ? 0 : -currentGravity, 
            controller.charCon.velocity.z);

        if (moveInput != Vector3.zero)
        {
            animator.transform.LookAt(animator.transform.position + moveInput);
        }
        
        // Update basic physics mechanics, regardless of the player's current state
        HandleGravity();

        HandleDirection();
        HandleJumping();

        HandleQuitting();
    }

    // Move the player vertically depending on how much ground is below them right now
    private void HandleGravity()
    {
        RaycastHit[] groundCheck = Physics.RaycastAll(transform.position, Vector3.down, Mathf.Infinity, controller.GroundLayer);

        // Let the player naturally fall at all times
        controller.charCon.Move(Vector3.down * (currentGravity * Time.deltaTime));
        if (!controller.charCon.isGrounded)
        {
            if (currentGravity <= controller.Gravity) 
            {
                if (currentJumpTimer >= controller.JumpTimer || currentJumpTimer <= 0)
                {
                    currentGravity += controller.GravityAccel;
                }
            }
        }
        
        foreach (RaycastHit hit in groundCheck)
        {
            // Then, check for objects which might manipulate the player's position/gravity
            bool exitLoop = false;
            switch (hit.collider.tag.ToString())
            {
                case "MovingPlatform":
                    // Use additional speed to make sure the player stays on the current platform
                    if (controller.charCon.isGrounded)
                    {
                        movingPlatformSpeed = hit.collider.GetComponent<MovingPlatform>().currentSpeed;
                        exitLoop = true;
                    }
                    break;

                case "Stairs":
                    // Use lots of gravity to ensure the player is stuck to the ground
                    if (controller.charCon.isGrounded) { controller.charCon.Move(Vector3.down * 100 * Time.deltaTime); }
                    break;

                case "Untagged":
                default:
                    if (!exitLoop)
                    {
                        movingPlatformSpeed = Vector3.zero;
                        if (controller.charCon.isGrounded) { currentGravity = 1; }
                    }
                    break;
            }

            if (exitLoop) break;
        }
    }

    // Reading all of the player's current inputs and updating all associated variables
    private void HandleInputs()
    {
        Vector2 vector2 = inputs.FindAction("Move").ReadValue<Vector2>();;
        moveInput = new Vector3(-vector2.x, 0, -vector2.y);
        runInput = inputs.FindAction("Run").IsPressed();

        jumpInput = inputs.FindAction("Jump").IsPressed();

        pickUpInput = inputs.FindAction("PickUp").triggered && inputs.FindAction("PickUp").ReadValue<float>() > 0;

        camRotInputLeft = inputs.FindAction("CamRotateLeft").IsPressed();
        camRotInputRight = inputs.FindAction("CamRotateRight").IsPressed();

        quitInput = inputs.FindAction("Quit").IsPressed();
    }

    // Find the appropriate direction vector for the player to move in
    private void HandleDirection()
    {
        if (moveInput != Vector3.zero)
        {
            direction = moveInput;
        }
    }

    private void HandleJumping()
    {
        if (!controller.charCon.isGrounded && jumpInput)
        {
            currentJumpTimer += Time.deltaTime;
        }
        else { currentJumpTimer = 0; }
    }

    private void HandleQuitting()
    {
        if (quitInput)
        {
            Debug.Log("Game is quit!");
            Application.Quit();
        }
    }

}
