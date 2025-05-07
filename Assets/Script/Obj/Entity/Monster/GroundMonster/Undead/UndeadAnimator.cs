using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UndeadState
{
    
    IDLE = 5,
    WALK = 0,
    DEAD = 6,
    ATTACK = 2,
    CHARGEATTACK = 1,
    FINISHATTACK = 3,
    HURT = 4
}
public class UndeadAnimator : EntityAnimator
{
    //==========================================Variable==========================================
    [Header("===Tree Sentinel===")]
    [SerializeField] protected Undead undead;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.undead, transform.parent, "LoadUndead()");
    }

    //===========================================Method===========================================
    protected override void HandlingStat()
    {
        // Move Speed
        if (this.undead.IsChasingTarget) this.animator.SetFloat("MoveSpeed", this.undead.ChaseSpeed / 5);
        else this.animator.SetFloat("MoveSpeed", this.undead.SlowSpeed / 5);

        // Cut
        this.animator.SetFloat("AttackSpeed", 0.2f/ this.undead.CutAttackCD.TimeLimit );
        this.animator.SetFloat("ChargeSpeed", 0.7f/ this.undead.CutChargeCD.TimeLimit);
        this.animator.SetFloat("FinishSpeed", 1.1f/ this.undead.CutFinishCD.TimeLimit);
    }

    protected override void HandlingState()

    {
        this.animator.SetInteger("State", (int)UndeadState.IDLE);

        if (this.undead.IsChasingTarget || this.undead.IsMovingRandomly)
        {
            this.animator.SetInteger("State", (int)UndeadState.WALK);
        }
        if (this.undead.Rb.velocity.x < 0.1 && this.undead.Rb.velocity.x > -0.1)  
        {
            this.animator.SetInteger("State", (int)UndeadState.IDLE);
        }
        if (this.undead.IsCutting && this.undead.CurrCutState == CutState.FINISH)
        {
            this.animator.SetInteger("State", (int)UndeadState.FINISHATTACK);
        }
        if (this.undead.IsCutting && this.undead.CurrCutState == CutState.ATTACK)
        {
            this.animator.SetInteger("State", (int)UndeadState.ATTACK);
        }
        if (this.undead.IsCutting && this.undead.CurrCutState == CutState.CHARGE)
        {
            this.animator.SetInteger("State", (int)UndeadState.CHARGEATTACK);
        }
        if (this.undead.IsAttackedPush == true)
        {
            this.animator.SetInteger("State", (int)UndeadState.HURT);
        }
        if (this.undead.Health <= 0)
        {
            this.animator.SetInteger("State", (int)UndeadState.DEAD);
        }
    }
}
