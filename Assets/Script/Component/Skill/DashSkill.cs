using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDashSkill
{
    // Property
    Rigidbody2D GetRb();
    int GetMoveDir();
    ref int GetDashDir();
    float GetDashSpeed();
    float GetGravityScale();
    Cooldown GetSkillCD();
    Cooldown GetDashCD();
    ref bool GetIsDashing();

    // Condition
    bool CanRechargeSkill();
    bool CanRechargeDash();
    bool CanDash();
}

public class DashSkill
{
    //===========================================Method===========================================
    public void Update(IDashSkill user)
    {
        this.RechargingDash(user);
        this.RechargingSkill(user);
        this.Dashing(user);
    }

    public void FinishDash(IDashSkill user)
    {
        user.GetIsDashing() = false;
        user.GetDashCD().ResetStatus();
        user.GetRb().gravityScale = user.GetGravityScale();
    }

    public void MakeSkillReady(IDashSkill user)
    {
        user.GetSkillCD().ResetStatus();
        user.GetIsDashing() = false;
        user.GetDashCD().ResetStatus();
    }

    //=======================================Recharge Skill=======================================
    private void RechargingSkill(IDashSkill user)
    {
        if (!user.CanRechargeSkill()) return;
        else if (user.GetIsDashing()) return; // is dashing
        this.RechargeSkill(user);
    }

    private void RechargeSkill(IDashSkill user)
    {
        user.GetSkillCD().CoolingDown();
    }

    //=======================================Recharge Dash========================================
    private void RechargingDash(IDashSkill user)
    {
        if (!user.CanRechargeDash()) return;
        else if (!user.GetIsDashing()) return; // is not dashing
        this.RechargeDash(user);
    }

    private void RechargeDash(IDashSkill user)
    {
        user.GetDashCD().CoolingDown();

        if (!user.GetDashCD().IsReady) return; // not finish recharge dash
        this.FinishDash(user);
    }

    //============================================Dash============================================
    private void Dashing(IDashSkill user)
    {
        if (user.GetIsDashing()) this.Dash(user);

        else
        {
            if (!user.CanDash()) return;
            if (!user.GetSkillCD().IsReady) return; // skill not ready
            user.GetDashDir() = user.GetMoveDir();
            this.Dash(user);
        }
    }

    private void Dash(IDashSkill user)
    {
        user.GetRb().velocity = new Vector2(user.GetDashSpeed() * user.GetDashDir(), 0);
        user.GetIsDashing() = true;
        user.GetSkillCD().ResetStatus();
        user.GetRb().gravityScale = 0;
    }
}
