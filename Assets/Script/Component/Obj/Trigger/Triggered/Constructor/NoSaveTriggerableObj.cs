using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoSaveTriggerableObj : TriggerableObj
{
    protected override void Awake()
    {
        base.Awake();
        this.NotTriggeredHandling();
    }
}
