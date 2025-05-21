using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Boss===")]
    [SerializeField] protected Cooldown appearCD;
    [SerializeField] protected bool isAppearing;

    //==========================================Get Set===========================================
    public Cooldown AppearCD => this.appearCD;
    public bool IsAppearing => this.isAppearing;

    protected override void OnEnable()
    {
        base.OnEnable();
        this.isAppearing = true;
        this.appearCD.ResetStatus();
    }

    //===========================================Appear===========================================
    protected virtual void Appearing()
    {
        this.appearCD.CoolingDown();

        if (!this.appearCD.IsReady) return;
        this.appearCD.ResetStatus();
        this.isAppearing = false;
    }

    protected override void Despawning()
    {
        base.Despawning();

        if (!this.despawnCD.IsReady) return;
        EventManager.Instance.OnBossDead?.Invoke();
    }
}
