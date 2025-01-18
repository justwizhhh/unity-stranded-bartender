using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : PlayerState
{
    public override void StartState()
    {
        
    }

    public override void UpdateState()
    {
        // Slowly resetting the player's speed after coming out of the 'walk'/'run' state, so it doesn't instantly get set to 0
        if (player.moveInput == Vector3.zero)
        {
            if (player.moveAccel >= controller.Acceleration) { player.moveAccel -= controller.Acceleration; }
            else player.moveAccel = 0;

            // Checking for jumping
            if (controller.charCon.isGrounded)
            {
                if (player.jumpInput)
                {
                    controller.stateMachine.ChangeState(typeof(PlayerJump));
                }
            }

            // Forwards and sideways movement respectively
            controller.charCon.Move(transform.forward * player.direction.z * player.moveAccel * Time.deltaTime);
            controller.charCon.Move(transform.right * player.direction.x * player.moveAccel * Time.deltaTime);
            controller.charCon.Move(player.movingPlatformSpeed);
        }
        else
        {
            controller.stateMachine.ChangeState(typeof(PlayerMove));
        }
    }

    public override void ExitState()
    {
        
    }
}
