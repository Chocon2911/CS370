using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTrigger_1 : ObjTrigger
{
    [SerializeField] protected Fly fly;

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.fly, transform.parent, "LoadFly()");
    }

    protected override void Register()
    {
        this.fly.OnDead += this.Trigger;
    }
}
