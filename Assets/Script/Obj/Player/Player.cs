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
    [SerializeField] protected PlayerAnimator playerAnimator;

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
    [SerializeField] protected float gravityScale;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected int dashDir;
    [SerializeField] protected bool isDashing;
    // Support
    [SerializeField] protected EffectSpawner.EffectType dashEffectType;
    [SerializeField] protected Transform tempDashEffect;
    [SerializeField] protected Vector2 dashEffectPos;

    [Header("Air Jump Skill")]
    [SerializeField] protected float airJumpSpeed;
    [SerializeField] protected bool isAirJumping;
    [SerializeField] protected bool isAirJump;

    //==========================================Get Set===========================================
    // ===Component===
    public Rigidbody2D Rb => this.rb;
    public CapsuleCollider2D BodyCol => this.bodyCol;
    public PlayerAnimator PlayerAnimator => this.playerAnimator;
    
    // ===Stat===
    public int MaxHealth => this.maxHealth;
    public int Health => this.health;
    public List<SkillType> UnlockedSkills => this.unlockedSkills;

    // ===Ground Check===
    public bool PrevIsGround => this.prevIsGround;
    public bool IsGround => this.isGround;

    // ===Move===
    public float MoveSpeed => this.moveSpeed;
    public int MoveDir => this.moveDir;
    public bool IsMoving => this.isMoving;

    // ===Jump===
    public float JumpSpeed => this.jumpSpeed;
    public bool IsJumping => this.isJumping;

    // ===Dash Skill===
    public Cooldown DashSkillCD => this.dashSkillCD;
    public Cooldown DashCD => this.dashCD;
    public float DashSpeed => this.dashSpeed;
    public int DashDir => this.dashDir;
    public bool IsDashing => this.isDashing;

    // ===Air Jump Skill===
    public float AirJumpSpeed => this.airJumpSpeed;
    public bool IsAirJumping => this.isAirJumping;
    public bool IsAirJump => this.isAirJump;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadChildComponent(ref this.groundCol, transform.Find("Ground"), "LoadGroundCol()");
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.LoadComponent(ref this.playerAnimator, transform.Find("Model"), "LoadAnimator()");
    }

    protected virtual void Update()
    {
        this.GroundChecking();
        this.Moving();
        this.Jumping();
        this.Facing();
        this.Dashing();
        SkillManager.Instance.AirJumpSkill.Update(this);
        this.playerAnimator.HandlingAnimator(this);

        // Dead
        if (this.health <= 0)
        {
            EventManager.Instance.OnPlayerDead?.Invoke();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.Instance.OnPlayerAppear?.Invoke();
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

        if (this.rb.velocity.x >= Mathf.Pow(1, 1) || this.rb.velocity.x <= -Mathf.Pow(1, 1) || this.moveDir != 0) this.isMoving = true;
        else this.isMoving = false;
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

    //============================================Dash============================================
    protected virtual void Dashing()
    {
        SkillManager.Instance.DashSkill.Update(this);

        if (this.isDashing && this.dashCD.Timer == 0)
        {
            float xPos = this.dashDir * this.dashEffectPos.x;
            Vector2 spawnPos = new Vector2(xPos, this.dashEffectPos.y);
            Quaternion spawnRot = Quaternion.identity;

            this.tempDashEffect = EffectSpawner.Instance.SpawnByType(this.dashEffectType, spawnPos, spawnRot);
            this.tempDashEffect.parent = this.transform;
            this.tempDashEffect.localPosition = spawnPos;
            this.tempDashEffect.gameObject.SetActive(true);
        }

        else if (this.tempDashEffect != null && !this.isDashing)
        {
            EffectSpawner.Instance.Despawn(this.tempDashEffect);
            this.tempDashEffect = null;
        }
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

    float IDashSkill.GetGravityScale()
    {
        return this.gravityScale;
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
            if (InputManager.Instance.SpaceState != 1) return false; // not press or hold space
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
