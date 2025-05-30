using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerableObj : DbObj
{
    //==========================================Variable==========================================
    [SerializeField] protected bool isTriggered;

    //===========================================Method===========================================
    protected abstract void NotTriggeredHandling();
    public abstract void Trigger();
}
