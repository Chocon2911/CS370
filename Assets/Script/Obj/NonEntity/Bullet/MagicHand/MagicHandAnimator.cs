using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHandAnimator : BaseAnimator
{
    //==========================================Variable==========================================
    [Header("===Magic Hand===")]
    [SerializeField] protected MagicHand magicHand;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.magicHand, transform.parent, "LoadMagicHand()");
    }

    //===========================================Method===========================================
    protected override void HandlingStat()
    {
        this.animator.SetFloat("AttackSpeed", 1 / this.magicHand.AttackCD.TimeLimit);
    }

    protected override void HandlingState()
    {
        this.animator.SetInteger("State", (int)this.magicHand.CurrAttackState);
    }
}
