using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableObj_Disappear : SaveTriggerableObj
{
    [SerializeField] protected Transform mainObj;

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.mainObj, transform.parent, "LoadMainObj()");
    }

    protected override void NotTriggeredHandling()
    {
        this.mainObj.gameObject.SetActive(true);
    }

    public override void Trigger()
    {
        this.isTriggered = true;
        this.mainObj.gameObject.SetActive(false);
    }
}
