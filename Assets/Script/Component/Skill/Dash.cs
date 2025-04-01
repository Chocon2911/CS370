using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDash
{
    // Property
    Rigidbody2D GetRb();
    int GetMoveDir();
    ref int GetDashDir();
    float GetDashSpeed();
    Cooldown GetSkillCD();
    Cooldown GetDashCD();
    ref bool GetIsDashing();

    // Condition
    bool CanRechargeSkill();
    bool CanRechargeDash();
    bool CanDash();
}

public class Dash
{
    //===========================================Method===========================================
    public void Update(IDash user)
    {
        this.RechargingDash(user);
        this.RechargingSkill(user);
        this.Dashing(user);
    }

    public void FinishDash(IDash user)
    {
        user.GetIsDashing() = false;
        user.GetDashCD().ResetStatus();
        user.GetRb().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }

    public void MakeSkillReady(IDash user)
    {
        user.GetSkillCD().ResetStatus();
        user.GetIsDashing() = false;
        user.GetDashCD().ResetStatus();
    }

    //=======================================Recharge Skill=======================================
    private void RechargingSkill(IDash user)
    {
        if (!user.CanRechargeSkill()) return;
        else if (user.GetIsDashing()) return; // is dashing
        this.RechargeSkill(user);
    }

    private void RechargeSkill(IDash user)
    {
        user.GetSkillCD().CoolingDown();
    }

    //=======================================Recharge Dash========================================
    private void RechargingDash(IDash user)
    {
        if (!user.CanRechargeDash()) return;
        else if (!user.GetIsDashing()) return; // is not dashing
        this.RechargeDash(user);
    }

    private void RechargeDash(IDash user)
    {
        user.GetDashCD().CoolingDown();

        if (!user.GetDashCD().IsReady) return; // not finish recharge dash
        this.FinishDash(user);
    }

    //============================================Dash============================================
    private void Dashing(IDash user)
    {
        if (user.GetIsDashing()) this.DoDash(user);

        else
        {
            if (!user.CanDash()) return;
            if (!user.GetSkillCD().IsReady) return; // skill not ready
            user.GetDashDir() = user.GetMoveDir();
            this.DoDash(user);
        }
    }

    private void DoDash(IDash user)
    {
        user.GetRb().velocity = new Vector2(user.GetDashSpeed() * user.GetDashDir(), 0);
        user.GetIsDashing() = true;
        user.GetSkillCD().ResetStatus();
        user.GetRb().constraints |= RigidbodyConstraints2D.FreezePositionY;
        user.GetRb().WakeUp();
    }
}
