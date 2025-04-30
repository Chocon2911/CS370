using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomManAnimator : EntityAnimator
{
    //==========================================Variable==========================================
    [Header("===Tree Sentinel===")]
    [SerializeField] protected MushroomMan mushroomMan;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.mushroomMan, transform.parent, "LoadMushroomMan()");
    }

    //===========================================Method===========================================
    protected override void HandlingStat()
    {
        if (this.mushroomMan.IsChasingTarget) this.animator.SetFloat("MoveSpeed", 1 / (this.mushroomMan.ChaseSpeed * 5));
        else this.animator.SetFloat("MoveSpeed", 1 / (this.mushroomMan.SlowSpeed * 5));
    }

    protected override void HandlingState()
    {
        throw new System.NotImplementedException();
    }
}
