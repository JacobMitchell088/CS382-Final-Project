using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    :base (currentContext, playerStateFactory){}
    public override void EnterState(){
      Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);  
      Ctx.Animator.SetBool(Ctx.IsRunningHash, true);
    }

    public override void UpdateState(){
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.RunMultiplier;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.RunMultiplier;
        DetectMovementDirection();
        CheckSwitchStates();
    }
    public override void ExitState(){}

    public override void InitalizeSubState(){}

    public override void CheckSwitchStates(){
        if (!Ctx.IsMovementPressed){
            SwitchState(Factory.Idle());
        } else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed) {
            SwitchState(Factory.Walk());
        }
    }
    private void DetectMovementDirection() {
        Ctx.Animator.SetFloat("mousePos", Ctx.PlayerYRot);  // Update directional rotation for the character

        // Handle directional movement animations based on input
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            SetAnimatorState(Ctx.WandAHash);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            SetAnimatorState(Ctx.WandDHash);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            SetAnimatorState(Ctx.SandAHash);
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            SetAnimatorState(Ctx.SandDHash);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            SetAnimatorState(Ctx.WHash);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            SetAnimatorState(Ctx.SHash);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            SetAnimatorState(Ctx.AHash);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            SetAnimatorState(Ctx.DHash);
        }
    }

    // Helper function to update animation states based on movement direction
    private void SetAnimatorState(int directionHash)
    {
        Ctx.Animator.SetBool(Ctx.WandAHash, directionHash == Ctx.WandAHash);
        Ctx.Animator.SetBool(Ctx.WandDHash, directionHash == Ctx.WandDHash);
        Ctx.Animator.SetBool(Ctx.SandAHash, directionHash == Ctx.SandAHash);
        Ctx.Animator.SetBool(Ctx.SandDHash, directionHash == Ctx.SandDHash);
        Ctx.Animator.SetBool(Ctx.WHash, directionHash == Ctx.WHash);
        Ctx.Animator.SetBool(Ctx.SHash, directionHash == Ctx.SHash);
        Ctx.Animator.SetBool(Ctx.AHash, directionHash == Ctx.AHash);
        Ctx.Animator.SetBool(Ctx.DHash, directionHash == Ctx.DHash);
    }
}
