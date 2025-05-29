using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArcherState
{
    IDLE = 0,
    RUN = 1,
    SHOOT = 2,
    DEAD = 3,
    HURT = 4,
}

public class ArcherAnimator : BaseAnimator
{
    //==========================================Variable==========================================
    [Header("===Archer===")]
    [SerializeField] protected Archer archer;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.archer, transform.parent, "LoadMonster()");
    }

    //===========================================Method===========================================
    protected override void HandlingStat()
    {
        this.animator.SetFloat("ChargeSpeed", 1 / this.archer.ChargBowCD.TimeLimit);
        this.animator.SetFloat("HurtSpeed", 1 / this.archer.HurtCD.TimeLimit);
        if (this.archer.IsMovingRandomly) this.animator.SetFloat("MoveSpeed", this.archer.SlowSpeed / 5);
        else this.animator.SetFloat("MoveSpeed", this.archer.ChaseSpeed / 5);
    }

    protected override void HandlingState()
    {
        this.animator.SetInteger("State", (int)ArcherState.IDLE);

        if (this.archer.IsMovingRandomly || this.archer.IsChasingTarget)
        {
            this.animator.SetInteger("State", (int)ArcherState.RUN);
        }

        if (this.archer.IsChargingBow)
        {
            this.animator.SetInteger("State", (int)ArcherState.SHOOT);
        }

        if (this.archer.IsHurting)
        {
            this.animator.SetInteger("State", (int)ArcherState.HURT);
        }

        if (this.archer.Health <= 0)
        {
            this.animator.SetInteger("State", (int)ArcherState.DEAD);
        }
    }
}
