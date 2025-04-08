using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAirJump
{
    // Property
    Rigidbody2D GetRb();
    float GetJumpSpeed();
    ref bool GetIsAirJumping();
    ref bool GetIsUsed();

    // Condition
    bool CanRestore();
    bool CanJump();

    // Addition
    void Enter();
}

public class AirJump
{
    //===========================================Method===========================================
    public void Update(IAirJump user)
    {
        this.RestoringSkill(user);
        this.Jumping(user);
        this.FinishingAirJump(user);
    }

    public void FinishAirJump(IAirJump user) 
    {
        MovementManager.Instance.StopJump(user.GetRb());
        user.GetIsAirJumping() = false;
    }

    //============================================Jump============================================
    private void Jumping(IAirJump user)
    {
        if (!user.CanJump()) return;
        if (user.GetIsAirJumping()) return; // is air jumping
        if (user.GetIsUsed()) return; // is used
        user.Enter();
        this.Jump(user);
    }

    private void Jump(IAirJump user)
    {
        user.GetRb().velocity = new Vector2(user.GetRb().velocity.x, user.GetJumpSpeed());
        user.GetIsAirJumping() = true;
        user.GetIsUsed() = true;
    }

    //=======================================Restore Skill========================================
    private void RestoringSkill(IAirJump user) 
    {
        if (!user.CanRestore()) return;
        if (!user.GetIsUsed()) return; // is used
        this.RestoreSkill(user);
    }

    private void RestoreSkill(IAirJump user)
    {
        user.GetIsUsed() = false;
    }

    //===========================================Finish===========================================
    private void FinishingAirJump(IAirJump user)
    {
        if (!user.GetIsAirJumping()) return;
        if (user.GetRb().velocity.y > 0) return; // is Jumping
        this.FinishAirJump(user);
    }
}
