using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeredTeleDoor_1 : TriggerableObj
{
    [SerializeField] protected TeleDoor teleDoor;

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.teleDoor, transform.parent, "LoadTeleDoor()");
    }

    public override void Trigger()
    {
        this.isTriggered = true;
        this.teleDoor.gameObject.SetActive(true);
    }

    protected override void NotTriggeredHandling()
    {
        this.teleDoor.gameObject.SetActive(false);
    }
}
