using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MushroomManState
{
    IDLE = 0,
    RUN = 1,
    DEAD = 2,
    ATTACK = 3,
}

public class MushroomManAnimator : BaseAnimator
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
        if (this.mushroomMan.IsChasingTarget) this.animator.SetFloat("MoveSpeed", this.mushroomMan.ChaseSpeed / 5);
        else this.animator.SetFloat("MoveSpeed", this.mushroomMan.SlowSpeed / 5);
    }

    protected override void HandlingState()
    {
        this.animator.SetInteger("State", (int)MushroomManState.IDLE);

        if (this.mushroomMan.IsChasingTarget || this.mushroomMan.IsMovingRandomly)
        {
            this.animator.SetInteger("State", (int)MushroomManState.RUN);
        }

        if (this.mushroomMan.IsBiting)
        {
            this.animator.SetInteger("State", (int)MushroomManState.ATTACK);
        }

        if (this.mushroomMan.Health <= 0)
        {
            this.animator.SetInteger("State", (int)MushroomManState.DEAD);
        }
    }
}
