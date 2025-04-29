using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityAnimator : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Entity===")]
    [SerializeField] protected Animator animator;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform, "LoadAnimator()");
    }

    //===========================================Method===========================================
    public virtual void HandlingAnimator()
    {
        this.HandlingStat();
        this.HandlingState();
    }

    protected abstract void HandlingStat();

    protected abstract void HandlingState();
}
