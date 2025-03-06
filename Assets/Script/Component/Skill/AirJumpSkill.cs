using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAirJumpSkill
{
    Rigidbody2D GetRb(AirJumpSkill component);
    bool CanUseSkill(AirJumpSkill component);
    bool GetIsUseSkill(AirJumpSkill component);
}

public class AirJumpSkill : Skill
{
    //==========================================Variable==========================================
    [Header("Double Jump")]
    [SerializeField] protected InterfaceReference<IAirJumpSkill> user1;
    [SerializeField] protected bool isJumped;

    [Header("Component")]
    [SerializeField] protected Jump jump;
    [SerializeField] protected GroundCheck groundCheck;

    //==========================================Get Set===========================================
    public IAirJumpSkill User1 { set => this.user1.Value = value; }

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.jump, transform.Find("Jump"), "LoadJump()");
        this.LoadComponent(ref this.groundCheck, transform.Find("GroundCheck"), "LoadGroundCheck()");
    }

    public override void MyUpdate()
    {
        base.MyUpdate();
        this.groundCheck.MyUpdate();
        this.Jumping();
        this.CheckingGround();
    }

    //============================================Jump============================================
    protected virtual void Jumping()
    {
        if (!this.user1.Value.CanUseSkill(this)) return;
        if (this.isJumped) return;
        if (!this.user1.Value.GetIsUseSkill(this)) return;
        this.Jump();
    }

    protected virtual void Jump()
    {
        this.jump.DoJump(this.user1.Value.GetRb(this));
        this.isJumped = true;
    }

    //========================================Check Ground========================================
    protected virtual void CheckingGround()
    {
        if (!this.groundCheck.IsGround) return;
        this.isJumped = false;
    }
}
