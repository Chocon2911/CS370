using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjTrigger : HuyMonoBehaviour
{
    [SerializeField] protected List<TriggerableObj> triggeredObjs = new List<TriggerableObj>();

    protected override void Awake()
    {
        base.Awake();
        this.Register();
    }

    protected abstract void Register();

    protected virtual void Trigger()
    {
        foreach (TriggerableObj triggeredObj in this.triggeredObjs)
        {
            triggeredObj.Trigger();
        }
    }
}
