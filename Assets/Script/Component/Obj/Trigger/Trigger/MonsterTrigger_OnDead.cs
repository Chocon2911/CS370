using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrigger_OnDead : ObjTrigger
{
    [SerializeField] protected Monster monster;

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.monster, transform.parent, "LoadMonster()");
    }

    protected override void Register()
    {
        this.monster.OnDead += this.Trigger;
    }
}
