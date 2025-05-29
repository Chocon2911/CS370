using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTrigger_1 : ObjTrigger
{
    //==========================================Variable==========================================
    [SerializeField] protected Bat bat;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.bat, transform.parent, "LoadBat()");
    }

    //===========================================Method===========================================
    protected override void Register()
    {
        this.bat.OnDead += this.Trigger;
    }
}
