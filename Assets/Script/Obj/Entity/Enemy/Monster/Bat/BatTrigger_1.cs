using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTrigger_1 : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] protected Bat bat;
    [SerializeField] protected InterfaceReference<TriggerableObj> triggerObj;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.bat, transform.parent, "LoadBat()");
    }

    protected override void Awake()
    {
        base.Awake();
        this.bat.OnDead += this.OnDead;
    }

    //===========================================Method===========================================
    protected virtual void OnDead()
    {
        this.triggerObj.Value.Trigger();
    }
}
