using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerState
{
    public override void StartState()
    {
        
    }

    public override void UpdateState()
    {
        // Moving the player itself
        if (player.moveInput.magnitude > 0)
        {
            // Checking for jumping
            if (controller.charCon.isGrounded)
            {
                if (player.jumpInput)
                {
                    controller.stateMachine.ChangeState(typeof(PlayerJump));
                }
            }

            // First, calculate the max speed the player can reach, determined by whether they are walking or running
            float max_speed = (player.runInput ? controller.RunSpeed : controller.WalkSpeed);
            player.moveAccel += controller.Acceleration * Time.deltaTime;
            player.moveAccel = Mathf.Clamp(player.moveAccel, 0, max_speed);

            // Then, rotate the player in their current movement
            Vector3 loweredCamPos = new Vector3(controller.virtualCamNoDamp.transform.position.x, player.transform.position.y, controller.virtualCamNoDamp.transform.position.z);
            transform.LookAt(loweredCamPos, Vector3.up);

            // Forwards and sideways movement respectively
            Vector3 finalMoveDirection = (transform.forward * player.direction.z + transform.right * player.direction.x) * player.moveAccel * Time.deltaTime + player.movingPlatformSpeed;
            controller.charCon.Move(finalMoveDirection);

            if (player.runInput)
            {
                player.animator.SetBool("Running", true);
                player.animator.SetBool("Walking", false);
            }
            else
            {
                player.animator.SetBool("Running", false);
                player.animator.SetBool("Walking", true);
            }
        }
        else
        {
            controller.stateMachine.ChangeState(typeof(PlayerIdle));
        }
    }

    public override void ExitState()
    {
        if (player.runInput)
        {
            player.animator.SetBool("Running", false);
        }
        else
        {
            player.animator.SetBool("Walking", false);
        }
    }
}
