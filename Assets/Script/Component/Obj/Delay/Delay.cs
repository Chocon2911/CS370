using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] private Cooldown cooldown;
    [SerializeField] private bool canRun = false;

    //===========================================Unity============================================
    private void Update()
    {
        if (!this.canRun || this.cooldown.IsReady) return;
        this.cooldown.CoolingDown();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.cooldown.ResetStatus();
        this.canRun = false;
    }

    //===========================================Method===========================================
    public void StartRun()
    {
        this.canRun = true;
    }

    public bool IsReady => this.cooldown.IsReady;
}
