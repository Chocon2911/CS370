using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDashSkill
{
    int GetDashDir(DashSkill component);
    Rigidbody2D GetRb(DashSkill component);
    bool GetIsDash(DashSkill component);
    bool CanRechargeSkill(DashSkill component);
    bool CanDash(DashSkill component);
}

public class DashSkill : Skill
{
    //==========================================Variable==========================================
    [Header("Dash")]
    [SerializeField] protected InterfaceReference<IDashSkill> user1;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected int dashDir;
    [SerializeField] protected Cooldown skillCD;
    [SerializeField] protected Cooldown dashCD;
    [SerializeField] protected bool isDashing;

    //==========================================Get Set===========================================
    public IDashSkill User1 { set => user1.Value = value; }
    public bool IsDashing => this.isDashing;

    public override void MyUpdate()
    {
        base.MyUpdate();
        this.RechargingSkill();
        this.Dashing();
    }

    //===========================================Skill============================================
    protected virtual void RechargingSkill()
    {
        if (!this.user1.Value.CanRechargeSkill(this)) return;
        if (this.isDashing) return;
        this.RechargeSkill();
    }

    protected virtual void RechargeSkill()
    {
        this.skillCD.CoolingDown();
    }

    //============================================Dash============================================
    protected virtual void Dashing()
    {
        if (!this.user1.Value.CanDash(this)) return;
        if (this.skillCD.IsReady && !this.isDashing)
        {
            this.isDashing = this.user1.Value.GetIsDash(this);
            this.dashDir = this.user1.Value.GetDashDir(this);
            if (this.dashDir == 0) this.isDashing = false; // Replace later with Character FaceDir
        }

        if (!this.isDashing) return;
        this.Dash();

        if (!this.dashCD.IsReady) return;
        this.FinishDash();
    }

    protected virtual void Dash()
    {
        Rigidbody2D rb = this.user1.Value.GetRb(this);
        float xVel = this.dashDir * this.dashSpeed;

        rb.velocity = new Vector2(xVel, 0);
        this.dashCD.CoolingDown();
    }

    protected virtual void FinishDash()
    {
        this.skillCD.ResetStatus();
        this.dashCD.ResetStatus();
        this.isDashing = false;
        this.user1.Value.GetRb(this).velocity = Vector2.zero;
    }
}
