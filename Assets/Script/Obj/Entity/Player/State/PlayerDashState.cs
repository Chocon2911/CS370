using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    //========================================Constructor=========================================
    public PlayerDashState(Player player) : base(player) { }

    //===========================================Method===========================================
    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        SkillManager.Instance.Dash.FinishDash(this.player);
    }

    public override void Update()
    {
        
    }
}
