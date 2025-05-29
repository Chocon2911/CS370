using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredBat_1 : TriggerableObj
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
    protected override void NotTriggeredHandling()
    {
        this.bat.gameObject.SetActive(true);
    }

    public override void Trigger()
    {
        this.isTriggered = true;
        this.bat.gameObject.SetActive(false);
    }
}
