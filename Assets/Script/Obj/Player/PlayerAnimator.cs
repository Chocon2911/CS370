using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerAnimator : HuyMonoBehaviour
{
    public enum PlayerAnimatorState
    {
        IDLE = 1,
        RUN = 2,
        JUMP = 3,
        FALL = 4,
        AIR_JUMP = 5,
        WALL_CLIMB = 6,
    }

    //==========================================Variable==========================================
    [SerializeField] private Animator animator;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform, "LoadAnimator()");
    }

    //===========================================Method===========================================
    public virtual void HandlingAnimator(Player player)
    {
        this.SetAnimatorState((int)PlayerAnimatorState.IDLE);

        // IS moving, ground, NOT jumping, dashing, air jumping
        if (player.IsMoving && player.IsGround && !player.IsJumping && !player.IsDashing && !player.IsAirJumping)
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
            this.SetAnimatorState((int)PlayerAnimatorState.IDLE); // Dash replace later
        }
    }

    protected virtual void SetAnimatorState(int state)
    {
        this.animator.SetInteger("State", state);
    }
}
