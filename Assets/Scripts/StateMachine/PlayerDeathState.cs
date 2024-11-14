using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    :base (currentContext, playerStateFactory){}
    
    public override void EnterState(){
      Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);  
      Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
      Ctx.Animator.SetBool(Ctx.IsDeadHash, true);
      Ctx.AppliedMovementX = 0;
      Ctx.AppliedMovementZ = 0;
    }

    public override void UpdateState(){}

    public override void ExitState(){}

    public override void InitalizeSubState(){}

    public override void CheckSwitchStates(){}
}
