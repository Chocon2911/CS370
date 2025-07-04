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
    WALL_CLIMB = 6,
    DEAD = 7,
    HURT = 8,
    APPEAR = 9,
}

public class PlayerAnimator : BaseAnimator
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
        this.animator.SetFloat("Move Speed", this.player.MoveSpeed / 5);
        this.animator.SetFloat("Hurt Speed", 1 / this.player.HurtCD.TimeLimit);
        this.animator.SetFloat("Appear Speed", 1 / this.player.AppearCD.TimeLimit);
        this.animator.SetFloat("Dead Speed", 1 / this.player.DespawnCD.TimeLimit - 0.2f);
    }
    protected override void HandlingState()
    {
        this.SetAnimatorState(PlayerAnimatorState.IDLE);

        if (this.player.IsHurting)
        {
            this.SetAnimatorState(PlayerAnimatorState.HURT);
        }

        // Is moving, ground, NOT jumping, dashing, air jumping
        else if (player.IsMoving && player.IsGround && !player.IsJumping && !player.IsDashing 
            && !player.IsAirJumping && !player.IsCastingEnergyBall)
        {
            this.SetAnimatorState(PlayerAnimatorState.RUN);
        }

        else if (player.IsJumping)
        {
            this.SetAnimatorState(PlayerAnimatorState.JUMP);
        }

        // yVelocity < 0, NOT ground
        else if (player.Rb.velocity.y < 0 && !player.IsGround)
        {
            this.SetAnimatorState(PlayerAnimatorState.FALL);
        }

        else if (player.IsAirJumping)
        {
            this.SetAnimatorState(PlayerAnimatorState.AIR_JUMP);
        }

        else if (player.IsDashing)
        {
            this.SetAnimatorState(PlayerAnimatorState.JUMP);
        }

        else if (player.IsCastingEnergyBall)
        {
            this.SetAnimatorState(PlayerAnimatorState.FALL);
            if (this.player.IsChargingEnergyBall) this.animator.SetInteger("Cast Energy Ball State", 0);
            else if (this.player.IsShootingEnergyBall) this.animator.SetInteger("Cast Energy Ball State", 1);
            else Debug.LogError("Cast Energy Ball Animation problem", transform.gameObject);
        }

        if (this.player.Health <= 0)
        {
            this.SetAnimatorState(PlayerAnimatorState.DEAD);
        }

        if (this.player.IsAppearing)
        {
            this.SetAnimatorState(PlayerAnimatorState.APPEAR);
        }

    }    

    protected virtual void SetAnimatorState(PlayerAnimatorState state)
    {
        this.animator.SetInteger("State", (int)state);
    }
}
