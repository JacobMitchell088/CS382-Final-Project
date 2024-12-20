using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    :base (currentContext, playerStateFactory){
        IsRootState = true;
    }

    public void handleGravity(){
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }

    public override void EnterState(){
        InitalizeSubState();
        handleGravity();
    }

    public override void UpdateState(){
        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void CheckSwitchStates(){
        // if player is grounded and jump is pressed, switch to jump state
        if (Ctx.IsJumpPressed && !Ctx.ReqiureNewJumpPress) {
            SwitchState(Factory.Jump());
        } else if (!Ctx.CharacterController.isGrounded) {
            SwitchState(Factory.Fall());
        }
        
    }

    public override void InitalizeSubState(){
        if(!Ctx.IsMovementPressed && !Ctx.IsRunPressed) {
            SetSubState(Factory.Idle());
        } else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed) {
            SetSubState(Factory.Walk());
        } else {
            SetSubState(Factory.Run());
        }
    }
}
