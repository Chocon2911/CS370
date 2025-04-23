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

public class PlayerAnimator : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] protected Player player; 
    [SerializeField] protected Animator animator;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform, "LoadAnimator()");
        this.LoadComponent(ref this.player, transform.parent, "LoadPlayer()");
    }

    //===========================================Method===========================================
    public virtual void HandlingAnimator(Player player)
    {
        this.HandleAnimation();
        this.HandleAnimationState();
    }

    protected virtual void HandleAnimation()
    {
        this.animator.SetFloat("Move Speed", this.player.MoveSpeed);
    }

    protected virtual void HandleAnimationState()
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
