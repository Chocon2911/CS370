using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyState
{
    WALK = 0,
    DEAD = 4,
    ATTACK = 1,
    HURT = 2,
    RUN = 3
}

public class FlyAnimator : BaseAnimator
{
    //==========================================Variable==========================================
    [Header("===Fly===")]
    [SerializeField] protected Fly fly;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.fly, transform.parent, "LoadFly()");
    }

    //===========================================Method===========================================
    protected override void HandlingStat()
    {
        this.animator.SetFloat("AttackSpeed", 1 / this.fly.ChargeFireCD.TimeLimit);
        if (this.fly.IsMovingRandomly) this.animator.SetFloat("MoveSpeed", this.fly.SlowSpeed / 5);
        else this.animator.SetFloat("RunSpeed", this.fly.RunAwaySpeed / 5);
    }

    protected override void HandlingState()
    {
        this.animator.SetInteger("State", (int)FlyState.WALK);

        
        if (this.fly.IsMovingRandomly)
        {
            this.animator.SetInteger("State", (int)FlyState.WALK);
        }else
        {
            this.animator.SetInteger("State", (int)FlyState.RUN);
        }
        if (this.fly.IsChargingFire)
        {
            this.animator.SetInteger("State", (int)FlyState.ATTACK);
        }
        if (this.fly.Health <= 0)
        {
            this.animator.SetInteger("State", (int)FlyState.DEAD);
        }
        //Chua lam Hurt
    }
}
