using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredFly_1 : TriggerableObj
{
    [SerializeField] protected Fly fly;

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.fly, transform.parent, "LoadFly()");
    }

    protected override void NotTriggeredHandling()
    {
        this.fly.gameObject.SetActive(true);
    }

    public override void Trigger()
    {
        this.isTriggered = true;
        this.fly.gameObject.SetActive(false);
    }
}
