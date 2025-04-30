using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArcherState
{
    IDLE = 0,
    RUN = 1,
    SHOOT = 2,
    DEAD = 3,
}

public class ArcherAnimator : EntityAnimator
{
    //==========================================Variable==========================================
    [Header("===Archer===")]
    [SerializeField] protected Archer monster;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.monster, transform.parent, "LoadMonster()");
    }

    //===========================================Method===========================================
    protected override void HandlingStat()
    {
        this.animator.SetFloat("ChargeSpeed", 1 / (2 * this.monster.ChargBowCD.TimeLimit));
        if (this.monster.IsMovingRandomly) this.animator.SetFloat("MoveSpeed", this.monster.SlowSpeed / 5);
        else this.animator.SetFloat("MoveSpeed", this.monster.ChaseSpeed / 5);
    }

    protected override void HandlingState()
    {
        this.animator.SetInteger("State", (int)ArcherState.IDLE);

        if (this.monster.IsMovingRandomly || this.monster.IsChasingTarget)
        {
            this.animator.SetInteger("State", (int)ArcherState.RUN);
        }

        if (this.monster.IsChargingBow)
        {
            this.animator.SetInteger("State", (int)ArcherState.SHOOT);
        }

        if (this.monster.Health <= 0)
        {
            this.animator.SetInteger("State", (int)ArcherState.DEAD);
        }
    }
}
