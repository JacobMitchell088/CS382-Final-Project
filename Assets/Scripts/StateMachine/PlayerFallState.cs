using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerStateMachine currnentContext, PlayerStateFactory playerStateFactory) 
    : base (currnentContext, playerStateFactory) {
        IsRootState = true;
    }

    public override void EnterState() {
        InitalizeSubState();
        Ctx.Animator.SetBool(Ctx.IsFallingHash, true);
    }

    public override void UpdateState() {
        handleGravity();
        CheckSwitchStates();
    }

    public override void ExitState() {
        Ctx.Animator.SetBool(Ctx.IsFallingHash, false);
    }

    public void handleGravity() {
        float previousYVelocity = Ctx.CurrentMovementY;
        Ctx.CurrentMovementY = Ctx.CurrentMovementY + Ctx.Gravity * Time.deltaTime;
        Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * 0.5f, -20.0f);
    }

    public override void CheckSwitchStates() {
        // if player is grounded, switch to the grounded state
        if (Ctx.CharacterController.isGrounded) {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitalizeSubState() {
        if(!Ctx.IsMovementPressed && !Ctx.IsRunPressed) {
            SetSubState(Factory.Idle());
        } else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed) {
            SetSubState(Factory.Walk());
        } else {
            SetSubState(Factory.Run());
        }
    }

}