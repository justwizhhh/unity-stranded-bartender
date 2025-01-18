using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpMove : PlayerMove
{
    public override void StartState()
    {
        if (controller.stateMachine.PreviousState.GetType() != typeof(PlayerJump))
        {
            player.currentGravity = -controller.JumpHeight;
            player.animator.SetBool("Jumping", true);
        }
    }

    public override void UpdateState()
    {
        if (controller.charCon.isGrounded)
        {
            controller.stateMachine.ChangeState(typeof(PlayerMove));
            player.animator.SetTrigger("Land");
            player.animator.SetBool("Jumping", false);
        }

        if (player.moveInput.magnitude > 0)
        {
            base.UpdateState();
        }
        else
        {
            controller.stateMachine.ChangeState(typeof(PlayerJump));
        }
    }

    public override void ExitState()
    {
        player.animator.SetBool("Running", false);
        player.animator.SetBool("Walking", false);
    }
}
