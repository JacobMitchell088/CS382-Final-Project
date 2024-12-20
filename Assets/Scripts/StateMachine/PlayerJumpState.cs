using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
        IEnumerator IJumpResetRoutine(){
        yield return new WaitForSeconds(.5f);
        Ctx.JumpCount = 0;
    }

    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    :base (currentContext, playerStateFactory){
        IsRootState = true;
    }

    public override void EnterState(){
        InitalizeSubState();
        handleJump();
    }

    public override void UpdateState(){
        handleGravity();
        CheckSwitchStates();
    }

    public override void ExitState(){
        Ctx.Animator.SetBool(Ctx.IsJumpingHash, false);
        if (Ctx.IsJumpPressed) {
            Ctx.ReqiureNewJumpPress = true;
        }
        Ctx.CurrentJumpResetRoutine = Ctx.StartCoroutine(IJumpResetRoutine());
        if (Ctx.JumpCount == 3){
            Ctx.JumpCount = 0;
            Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
        }
    }

    public override void CheckSwitchStates(){
        if (Ctx.CharacterController.isGrounded) {
            SwitchState(Factory.Grounded());
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

    void handleJump(){
        if (Ctx.JumpCount < 3 && Ctx.CurrentJumpResetRoutine != null){
              Ctx.StopCoroutine(Ctx.CurrentJumpResetRoutine);
            }    
            Ctx.Animator.SetBool(Ctx.IsJumpingHash, true);
            Ctx.IsJumping = true;
            Ctx.JumpCount += 1;
            Ctx.Animator.SetInteger(Ctx.JumpCountHash, Ctx.JumpCount);
            Ctx.CurrentMovementY = Ctx.InitalJumpVelocities[Ctx.JumpCount];
            Ctx.AppliedMovementY = Ctx.InitalJumpVelocities[Ctx.JumpCount];
        } 

    
    public void handleGravity(){
        bool isFalling = Ctx.CurrentMovementY <= 0.0f || Ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;

        if (isFalling){
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * .5f, -20.0f);
        } 
        else {
            float previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY = Ctx.CurrentMovementY + (Ctx.JumpGravities[Ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = (previousYVelocity + Ctx.CurrentMovementY) * .5f;
        }
    }
}

