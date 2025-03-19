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
    [SerializeField] protected List<SkillType> unlockedSkills;

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
    [SerializeField] protected Cooldown jumpStartCD;
    [SerializeField] protected bool isJumping;

    [Header("Dash Skill")]
    [SerializeField] protected Cooldown dashSkillCD;
    [SerializeField] protected Cooldown dashCD;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected int dashDir;
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
        if (this.isGround) this.jumpStartCD.ResetStatus();

        if (!this.isDashing)
        {
            bool isSpacePressed = InputManager.Instance.SpaceState != 0;
            MovementManager movment = MovementManager.Instance;

            if (this.isGround && isSpacePressed && !this.isJumping)
            {
                this.isJumping = true;
                movment.Jump(this.rb, this.jumpSpeed);
            }

            else if (this.isJumping)
            {
                if (!this.jumpStartCD.IsReady) this.jumpStartCD.CoolingDown();
                else
                {
                    if (!isSpacePressed) movment.StopJump(this.rb);
                    if (this.rb.velocity.y <= 0) this.isJumping = false;
                }
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
        this.moveDir = (int)InputManager.Instance.MoveDir.x;
        if (this.moveDir != 0) return this.moveDir;
        if (transform.eulerAngles.y == 0) return 1;
        else return -1;
    }

    ref int IDashSkill.GetDashDir()
    {
        return ref this.dashDir;
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
        foreach (SkillType unlockedSkill in this.unlockedSkills)
        {
            if (unlockedSkill != SkillType.DASH) continue;
            if (this.dashSkillCD.Timer > 0) return true; // already in recharging
            if (this.isGround) return true; // is ground
        }
        return false;
    }

    bool IDashSkill.CanRechargeDash()
    {
        foreach (SkillType unlockedSkill in this.unlockedSkills)
        {
            if (unlockedSkill != SkillType.DASH) continue;
            return true;
        }
        return false;
    }

    bool IDashSkill.CanDash()
    {
        foreach (SkillType unlockedSkill in this.unlockedSkills)
        {
            if (unlockedSkill != SkillType.DASH) continue;
            if (InputManager.Instance.ShiftState != 0) return true; // press or hold shift
            return false;
        }
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
        foreach (SkillType unlockedSkill in this.unlockedSkills)
        {
            if (unlockedSkill != SkillType.AIR_JUMP) continue;
            return true;
        }
        return false;
    }

    bool IAirJumpSkill.CanJump()
    {
        foreach (SkillType skillType in this.unlockedSkills)
        {
            if (skillType != SkillType.AIR_JUMP) continue;
            if (InputManager.Instance.SpaceState == 0) return false; // not press or hold space
            if (this.isGround) return false; // is ground
            if (this.isJumping) return false; // is jumping
            if (this.isDashing) return false; // is dashing
            return true;
        }
        return false;
    }

    bool IAirJumpSkill.CanFinishAirJump()
    {
        foreach (SkillType unlockedSkill in this.unlockedSkills)
        {
            if (unlockedSkill != SkillType.AIR_JUMP) continue;
            return true;
        }
        return false;
    }

    bool IAirJumpSkill.GetIsGround()
    {
        return this.isGround;
    }
}
