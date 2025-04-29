using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PlayerAnimatorState
{
    IDLE = 1,
    RUN = 2,
    JUMP = 3,
    FALL = 4,
    AIR_JUMP = 5,
    DASH = 6,
    CAST_ENERGY_BALL = 7,
    REST = 8,
}

public class PlayerAnimator : EntityAnimator
{
    //==========================================Variable==========================================
    [SerializeField] protected Player player;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.player, transform.parent, "LoadPlayer()");
    }

    //===========================================Method===========================================
    protected override void HandlingStat()
    {
        this.animator.SetFloat("Move Speed", this.player.MoveSpeed);
    }
    protected override void HandlingState()
    {
        this.SetAnimatorState((int)PlayerAnimatorState.IDLE);

        // Is moving, ground, NOT jumping, dashing, air jumping
        if (player.IsMoving && player.IsGround && !player.IsJumping && !player.IsDashing && !player.IsAirJumping && !player.IsCastingEnergyBall)
        {
            this.SetAnimatorState((int)PlayerAnimatorState.RUN);
        }

        else if (player.IsJumping)
        {
            this.SetAnimatorState((int)PlayerAnimatorState.JUMP);
        }

        // yVelocity < 0, NOT ground
        else if (player.Rb.velocity.y < 0 && !player.IsGround)
        {
            this.SetAnimatorState((int)PlayerAnimatorState.FALL);
        }

        else if (player.IsAirJumping)
        {
            this.SetAnimatorState((int)PlayerAnimatorState.AIR_JUMP);
        }

        else if (player.IsDashing)
        {
            this.SetAnimatorState((int)PlayerAnimatorState.DASH);
        }

        else if (player.IsCastingEnergyBall)
        {
            this.SetAnimatorState((int)PlayerAnimatorState.CAST_ENERGY_BALL);
            if (this.player.IsChargingEnergyBall) this.animator.SetInteger("Cast Energy Ball State", 0);
            else if (this.player.IsShootingEnergyBall) this.animator.SetInteger("Cast Energy Ball State", 1);
            else Debug.LogError("Cast Energy Ball Animation problem", transform.gameObject);
        }

        if (this.player.IsRest)
        {
            this.SetAnimatorState((int)PlayerAnimatorState.REST);
        }
    }    

    protected virtual void SetAnimatorState(int state)
    {
        this.animator.SetInteger("State", state);
    }
}
