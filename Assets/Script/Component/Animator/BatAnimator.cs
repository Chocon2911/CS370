using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BatState
{
    FLY = 0,
    DEAD = 1,
    GORE = 2,
    HURT = 3,
}

public class BatAnimator : BaseAnimator
{
    //==========================================Variable==========================================
    [Header("===Bat===")]
    [SerializeField] protected Bat bat;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.bat, transform.parent, "LoadBat()");
    }

    //===========================================Method===========================================
    protected override void HandlingStat()
    {
        this.animator.SetFloat("ChargeSpeed", 1 / this.bat.GoreChargeCD.TimeLimit);
        this.animator.SetFloat("HurtSpeed", 1 / this.bat.HurtCD.TimeLimit);
        if (this.bat.IsChasingTarget) this.animator.SetFloat("MoveSpeed", this.bat.ChaseSpeed / 5);
        else this.animator.SetFloat("MoveSpeed", this.bat.SlowSpeed / 5);
    }

    protected override void HandlingState()
    {
        this.animator.SetInteger("State", (int)BatState.FLY);

        if (this.bat.IsGoring)
        {
            this.animator.SetInteger("State", (int)BatState.GORE);
        }
        
        if (this.bat.IsHurting)
        {
            this.animator.SetInteger("State", (int)BatState.HURT);
        }
        
        if (this.bat.Health <= 0)
        {
            this.animator.SetInteger("State", (int)BatState.DEAD);
        }
    }
}
