using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAirJumpSkill
{
    // Property
    Rigidbody2D GetRb();
    float GetJumpSpeed();
    ref bool GetIsAirJumping();
    ref bool GetIsUsed();

    // Condition
    bool CanRestoreSkill();
    bool CanJump();
    bool CanFinishAirJump();
    bool GetIsGround();
}

public class AirJumpSkill
{
    //===========================================Method===========================================
    public void Update(IAirJumpSkill user)
    {
        this.RestoringSkill(user);
        this.Jumping(user);
        this.FinishingAirJump(user);
    }

    public void FinishAirJump(IAirJumpSkill user) 
    {
        user.GetIsAirJumping() = false;
    }

    public void Jump(IAirJumpSkill user)
    {
        user.GetRb().velocity = new Vector2(user.GetRb().velocity.x, user.GetJumpSpeed());
        user.GetIsAirJumping() = true;
        user.GetIsUsed() = true;
    }

    public void RestoreSkill(IAirJumpSkill user)
    {
        user.GetIsUsed() = false;
    }

    //============================================Jump============================================
    private void Jumping(IAirJumpSkill user)
    {
        if (!user.CanJump()) return;
        if (user.GetIsAirJumping()) return; // is air jumping
        if (user.GetIsUsed()) return; // is used
        this.Jump(user);
    }

    //=======================================Restore Skill========================================
    private void RestoringSkill(IAirJumpSkill user) 
    {
        if (!user.CanRestoreSkill()) return;
        if (!user.GetIsGround()) return; // is not ground
        if (!user.GetIsUsed()) return; // is used
        this.RestoreSkill(user);
    }

    //===========================================Finish===========================================
    private void FinishingAirJump(IAirJumpSkill user)
    {
        if (!user.GetIsAirJumping()) return;
        if (user.GetRb().velocity.y > 0) return; // is Jumping
        this.FinishAirJump(user);
    }
}
