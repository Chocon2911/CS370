using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableObj_Appear : SaveTriggerableObj
{
    //==========================================Variable==========================================
    [SerializeField] protected Transform mainObj;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.mainObj, transform.parent, "LoadDoor()");
    }

    //===========================================Method===========================================
    protected override void NotTriggeredHandling()
    {
        this.mainObj.gameObject.SetActive(false);
    }

    public override void Trigger()
    {
        this.isTriggered = true;
        this.mainObj.gameObject.SetActive(true);
    }
}
