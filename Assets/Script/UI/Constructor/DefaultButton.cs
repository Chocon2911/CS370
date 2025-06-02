using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BtnState
{
    NONE = 0,
    PRESSED = 1,
    HOLD = 2,
}

public class DefaultButton : HuyMonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //==========================================Variable==========================================
    [Header("===Default Button===")]
    [SerializeField] protected int state;
    [SerializeField] protected Cooldown holdCD;

    //==========================================Get Set===========================================
    public int State => state;

    //===========================================Unity============================================
    protected override void Awake()
    {
        base.Awake();
        this.state = (int)BtnState.NONE;
        this.holdCD.ResetStatus();
    }

    protected virtual void Update()
    {
        this.Holding();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        this.OnBtnHold();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        this.OnBtnRelease();
    }

    //===========================================Method===========================================
    protected virtual void OnBtnHold()
    {
        if (!this.holdCD.IsReady) this.state = (int)BtnState.PRESSED;
        else this.state = (int)BtnState.HOLD;
        
    }

    protected virtual void OnBtnRelease()
    {
        this.state = (int)BtnState.NONE;
        this.holdCD.ResetStatus();
    }

    protected virtual void Holding()
    {
        if (this.holdCD.IsReady || this.state != (int)BtnState.PRESSED) return;
        this.holdCD.CoolingDown();
    }
}
