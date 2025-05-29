using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredDoor_1 : TriggerableObj
{
    //==========================================Variable==========================================
    [SerializeField] protected Door door;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.door, transform.parent, "LoadDoor()");
    }

    //===========================================Method===========================================
    protected override void NotTriggeredHandling()
    {
        this.door.gameObject.SetActive(false);
    }

    public override void Trigger()
    {
        this.isTriggered = true;
        this.door.gameObject.SetActive(true);
    }
}
