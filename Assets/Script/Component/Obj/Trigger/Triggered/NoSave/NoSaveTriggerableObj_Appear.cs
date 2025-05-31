using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSaveTriggerableObj_Appear : NoSaveTriggerableObj
{
    [SerializeField] protected Transform mainObj;

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.mainObj, transform.parent, "LoadMainObj()");
    }

    protected override void NotTriggeredHandling()
    {
        this.isTriggered = false;
        this.mainObj.gameObject.SetActive(false);
    }

    public override void Trigger()
    {
        this.isTriggered = true;
        this.mainObj.gameObject.SetActive(true);
    }
}
