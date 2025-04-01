using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastEnergyBallState : PlayerState
{
    //==========================================Variable==========================================
    [Header("===Cast Energy Ball===")]
    [SerializeField] private CastEnergyBallData data;

    //========================================Constructor=========================================
    public PlayerCastEnergyBallState(Player player) : base(player) { }

    //===========================================Method===========================================
    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        //SkillManager.Instance.CastEBall.
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}
