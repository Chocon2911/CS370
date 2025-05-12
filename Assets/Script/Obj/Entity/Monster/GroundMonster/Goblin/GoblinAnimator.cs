using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GoblinState
{

    IDLE = 5,
    WALK = 0,
    DEAD = 6,
    ATTACK = 2,
    CHARGEATTACK = 1,
    FINISHATTACK = 3,
    HURT = 4
}
public class GoblinAnimator : EntityAnimator
{
    //==========================================Variable==========================================
    [Header("===Tree Sentinel===")]
    [SerializeField] protected Goblin goblin;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.goblin, transform.parent, "LoadUndead()");
    }

    //===========================================Method===========================================
    protected override void HandlingStat()
    {
        // Move Speed
        if (this.goblin.IsChasingTarget) this.animator.SetFloat("MoveSpeed", this.goblin.ChaseSpeed / 5);
        else this.animator.SetFloat("MoveSpeed", this.goblin.SlowSpeed / 5);

        // Cut
        this.animator.SetFloat("AttackSpeed", 0.2f / this.goblin.HitAttackCD.TimeLimit);
        this.animator.SetFloat("ChargeSpeed", 0.2f / this.goblin.HitChargeCD.TimeLimit);
        this.animator.SetFloat("FinishSpeed", 0.3f / this.goblin.HitFinishCD.TimeLimit);
    }

    protected override void HandlingState()

    {
        this.animator.SetInteger("State", (int)GoblinState.IDLE);

        if (this.goblin.IsChasingTarget || this.goblin.IsMovingRandomly)
        {
            this.animator.SetInteger("State", (int)GoblinState.WALK);
        }
        if (this.goblin.Rb.velocity.x < 0.1 && this.goblin.Rb.velocity.x > -0.1)
        {
            this.animator.SetInteger("State", (int)GoblinState.IDLE);
        }
        if (this.goblin.IsHitting && this.goblin.CurrHitState == HitState.FINISH)
        {
            this.animator.SetInteger("State", (int)GoblinState.FINISHATTACK);
        }
        if (this.goblin.IsHitting && this.goblin.CurrHitState == HitState.ATTACK)
        {
            this.animator.SetInteger("State", (int)GoblinState.ATTACK);
        }
        if (this.goblin.IsHitting && this.goblin.CurrHitState == HitState.CHARGE)
        {
            this.animator.SetInteger("State", (int)GoblinState.CHARGEATTACK);
        }
        if (this.goblin.IsAttackedPush == true)
        {
            this.animator.SetInteger("State", (int)GoblinState.HURT);
        }
        if (this.goblin.Health <= 0)
        {
            this.animator.SetInteger("State", (int)GoblinState.DEAD);
        }
    }
}