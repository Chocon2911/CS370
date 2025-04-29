using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    RUN = 0,
    SHOOT = 1,
    DEAD = 2,
}

public class MonsterAnimator : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] protected Monster monster;
    [SerializeField] protected Animator animator;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.monster, transform.parent, "LoadMonster()");
        this.LoadComponent(ref this.animator, transform, "LoadAnimator()");
    }

    //===========================================Method===========================================
    public virtual void HandlingAnimator()
    {
        this.HandingStat();
        this.HandlingState();
    }
    
    protected virtual void HandingStat()
    {
        this.animator.SetFloat("ChargeSpeed", 1 / (2 * this.monster.ChargBowCD.TimeLimit));
        if (this.monster.IsWalking) this.animator.SetFloat("MoveSpeed", this.monster.WalkSpeed / 5);
        else this.animator.SetFloat("MoveSpeed", this.monster.ChaseSpeed / 5);
    }

    protected virtual void HandlingState()
    {
        this.animator.SetInteger("State", 0);

        if (this.monster.IsWalking || this.monster.IsChasingTarget)
        {
            this.animator.SetInteger("State", (int)MonsterState.RUN);
        }

        if (this.monster.IsChargingBow)
        {
            this.animator.SetInteger("State", (int)MonsterState.SHOOT);
        }

        if (this.monster.Health <= 0)
        {
            this.animator.SetInteger("State", (int)MonsterState.DEAD);
        }
    }
}
