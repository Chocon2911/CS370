using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class NonDbItem : HuyMonoBehaviour, ItemComponent, FlyUpUser
{
    //==========================================Variable==========================================
    [Header("Component")]
    [SerializeField] private ItemSO so;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D bodyCol;
    [SerializeField] private Delay delayBeforePickable;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.LoadComponent(ref this.delayBeforePickable, transform.Find("DelayBeforePickable"), "LoadDelayBeforePickable()");

        this.DefaultStat();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Debug.Log("Fuck off");
        if (this.delayBeforePickable != null) this.delayBeforePickable.StartRun();
    }

    //===========================================Method===========================================
    private void DefaultStat()
    {
        this.rb.isKinematic = false;
        this.bodyCol.isTrigger = true;
    }

    //=======================================Item Component=======================================
    void ItemComponent.PickedUp(ItemUser user)
    {
        if (this.delayBeforePickable != null && !this.delayBeforePickable.IsReady) return;
        user.AddHealth(this.so.RestoredHealth);
        user.UnlockSkill(this.so.UnlockedSkill);
        user.AddCoin(this.so.CoinAmount);
        Debug.Log("WTF");
        gameObject.SetActive(false);
    }

    //========================================Fly Up User=========================================
    Rigidbody2D FlyUpUser.GetRb()
    {
        return this.rb;
    }
}
