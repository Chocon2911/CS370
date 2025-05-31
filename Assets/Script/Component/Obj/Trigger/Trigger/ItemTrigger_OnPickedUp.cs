using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger_OnPickedUp : ObjTrigger
{
    //==========================================Variable==========================================
    [SerializeField] protected Item item; 

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.item, transform.parent, "LoadItem()");
    }

    //===========================================Method===========================================
    protected override void Register()
    {
        this.item.OnPickedUp += this.Trigger;
    }

    protected override void Trigger()
    {
        Time.timeScale = 0f;
        base.Trigger();
    }
}
