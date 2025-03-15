using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : HuyMonoBehaviour, IDashSkill, IAirJumpSkill
{
    //==========================================Variable==========================================
    [Header("===Player===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D bodyCol;

    [Header("Stat")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;
    [SerializeField] protected bool hasDashSkill;
    [SerializeField] protected bool hasAirJumpSkill;

    [Header("Ground Check")]
    [SerializeField] protected CapsuleCollider2D groundCol;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected string groundTag = "Ground";
    [SerializeField] protected bool prevIsGround;
    [SerializeField] protected bool isGround;

    [Header("Move")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveSpeedUpTime;
    [SerializeField] protected float moveSlowDownTime;
    [SerializeField] protected int moveDir;
    [SerializeField] protected bool isMoving;

    [Header("Jump")]
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected bool isJumping;
    [SerializeField] protected bool isJump;

    [Header("Dash Skill")]
    [SerializeField] protected Cooldown dashSkillCD;
    [SerializeField] protected Cooldown dashCD;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected bool isDashing;

    [Header("Air Jump Skill")]
    [SerializeField] protected float airJumpSpeed;
    [SerializeField] protected bool isAirJumping;
    [SerializeField] protected bool isAirJump;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadChildComponent(ref this.groundCol, transform.Find("Ground"), "LoadGroundCol()");
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
    }

    protected virtual void Update()
    {
        this.GroundChecking();
        this.Moving();
        this.Jumping();
        this.Facing();
        SkillManager.Instance.DashSkill.Update(this);
        SkillManager.Instance.AirJumpSkill.Update(this);
    }



    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //========================================Ground Check========================================
    protected virtual void GroundChecking()
    {
        Vector2 size = this.groundCol.size;
        Vector2 pos = this.groundCol.transform.position;
        CapsuleDirection2D dir = this.groundCol.direction;
        float angle = 0;

        Collider2D[] targetCols = Physics2D.OverlapCapsuleAll(pos, size, dir, angle, this.groundLayer);

        foreach (Collider2D targetCol in targetCols)
        {
            if (targetCol.tag != this.groundTag) continue;
            this.prevIsGround = this.isGround;
            this.isGround = true;
            return;
        }

        this.prevIsGround = this.isGround;
        this.isGround = false;
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
        if (!this.isDashing)
        {
            this.moveDir = (int)InputManager.Instance.MoveDir.x;
            MovementManager.Instance.Move(this.rb, this.moveDir, this.moveSpeed, this.moveSpeedUpTime, this.moveSlowDownTime);
        }
    }

    //============================================Jump============================================
    protected virtual void Jumping()
    {
        if (this.isGround) this.isJump = false;

        if (!this.isDashing)
        {
            bool isSpacePressed = InputManager.Instance.SpaceState != 0;
            MovementManager movment = MovementManager.Instance;

            if (this.isGround && isSpacePressed && !this.isJumping)
            {
                this.isJumping = true;
                movment.Jump(this.rb, this.jumpSpeed);
            }
            else if (!isSpacePressed && this.isJumping) movment.StopJump(this.rb);

            if (this.rb.velocity.y <= Mathf.Pow(0.1f, 3))
            {
                this.isJumping = false;
                this.isJump = true;
            }
        }
    }

    //============================================Face============================================
    protected virtual void Facing()
    {
        if (this.moveDir == 0) return;
        UtilManager.Instance.RotateFaceDir(this.moveDir, this.transform);
    }



    //============================================================================================
    //=========================================Interface==========================================
    //============================================================================================

    //=========================================Dash Skill=========================================
    // ===Property===
    Rigidbody2D IDashSkill.GetRb()
    {
        return this.rb;
    }

    int IDashSkill.GetMoveDir()
    {
        return this.moveDir;
    }

    float IDashSkill.GetDashSpeed()
    {
        return this.dashSpeed;
    }

    Cooldown IDashSkill.GetSkillCD()
    {
        return this.dashSkillCD;
    }

    Cooldown IDashSkill.GetDashCD()
    {
        return this.dashCD;
    }

    ref bool IDashSkill.GetIsDashing()
    {
        return ref this.isDashing;
    }

    // ===Condition===
    bool IDashSkill.CanRechargeSkill()
    {
        if (!this.hasDashSkill) return false; // if not have skill
        if (this.dashSkillCD.Timer > 0) return true; // If already in recharging
        if (this.isGround) return true; // if is ground
        return false;
    }

    bool IDashSkill.CanRechargeDash()
    {
        if (!this.hasDashSkill) return false; // if not have skill
        return true;
    }

    bool IDashSkill.CanDash()
    {
        if (!this.hasDashSkill) return false; // if not have skill
        if (InputManager.Instance.ShiftState != 0) return true; // if press or hold shift
        return false;
    }

    //=======================================Air Jump Skill=======================================
    // ===Property===
    Rigidbody2D IAirJumpSkill.GetRb()
    {
        return this.rb;
    }

    float IAirJumpSkill.GetJumpSpeed()
    {
        return this.airJumpSpeed;
    }

    ref bool IAirJumpSkill.GetIsAirJumping()
    {
        return ref this.isAirJumping;
    }

    ref bool IAirJumpSkill.GetIsUsed()
    {
        return ref this.isAirJump;
    }

    // ===Condition===
    bool IAirJumpSkill.CanRestoreSkill()
    {
        if (!this.hasAirJumpSkill) return false; // not have skill
        return true;
    }

    bool IAirJumpSkill.CanJump()
    {
        if (!this.hasAirJumpSkill) return false; // not have skill
        if (InputManager.Instance.SpaceState == 0) return false; // not press or hold space
        if (this.isGround) return false; // is ground
        if (this.isJumping) return false; // is jumping
        if (this.isDashing) return false; // is dashing
        return true;
    }

    bool IAirJumpSkill.CanFinishAirJump()
    {
        if (!this.hasAirJumpSkill) return false; // not have skill
        return true;
    }

    bool IAirJumpSkill.GetIsGround()
    {
        return this.isGround;
    }
}
