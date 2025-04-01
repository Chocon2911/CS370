using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirJumpState : PlayerState
{
    //========================================Constructor=========================================
    public PlayerAirJumpState(Player player) : base(player) { }

    //===========================================Method===========================================
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        SkillManager.Instance.AirJump.FinishAirJump(this.player);
    }

    public override void Update()
    {
        
    }
}
