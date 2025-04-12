using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity, IDash, IAirJump, ICastEnergyBall, Damagable, ControllableByDoor, DoorUser
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Player===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D bodyCol;
    [SerializeField] protected PlayerAnimator playerAnimator;
    [SerializeField] protected PlayerSO so;
    [SerializeField] protected Transform shootPoint;

    [Space(25)]
    
    [Header("Stat")]
    [SerializeField] protected bool hasDash;
    [SerializeField] protected bool hasAirJump;
    [SerializeField] protected bool hasCastEnergyBall;

    [Space(25)]

    [Header("Ground Check")]
    [SerializeField] protected CapsuleCollider2D groundCol;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected string groundTag = "Ground";
    [SerializeField] protected bool prevIsGround;
    [SerializeField] protected bool isGround;

    [Space(25)]

    [Header("Interact Check")]
    [SerializeField] protected float interactDetectLength;
    [SerializeField] protected Transform interactableObj;

    [Space(25)]

    [Header("Move")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveSpeedUpTime;
    [SerializeField] protected float moveSlowDownTime;
    [SerializeField] protected int moveDir;
    [SerializeField] protected bool isMoving;

    [Space(25)]

    [Header("Jump")]
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected Cooldown jumpStartCD;
    [SerializeField] protected bool isJumping;

    [Space(25)]

    [Header("Dash")]
    [SerializeField] protected DashData dash;
    // Support
    [SerializeField] protected TrailRenderer dashTrail;

    [Space(25)]

    [Header("Air Jump")]
    [SerializeField] protected AirJumpData airJump;

    [Space(25)]

    [Header("Cast Energy Ball")]
    [SerializeField] protected CastEnergyBallData castEnergyBall;

    //==========================================Get Set===========================================
    // ===Component===
    public Rigidbody2D Rb => this.rb;
    public CapsuleCollider2D BodyCol => this.bodyCol;

    // ===Ground Check===
    public bool PrevIsGround => this.prevIsGround;
    public bool IsGround => this.isGround;

    // ===Move===
    public bool IsMoving => this.isMoving;

    // ===Jump===
    public bool IsJumping => this.isJumping;

    // ===Dash Skill===
    public bool IsDashing => this.dash.isDashing;

    // ===Air Jump Skill===
    public bool IsAirJumping => this.airJump.isJumping;
    public bool IsAirJump => this.airJump.isJump;

    // ===Cast Energy Ball===
    public bool IsCastingEnergyBall => this.castEnergyBall.isCasting;
    
    // ===Db===
    public PlayerDbData PlayerDbData
    {
        get => new PlayerDbData(transform.position, transform.rotation, this.id, this.health, this.dash.dashCD.Timer, this.castEnergyBall.restoreCD.Timer);
        set
        {
            this.transform.position = new Vector3(value.xPos, value.yPos, value.zPos);
            this.transform.rotation = Quaternion.Euler(value.xRot, value.yRot, value.zRot);
            this.id = value.id;
            this.health = value.health;
            this.dash.dashCD.Timer = value.dashRestoreTimer;
            this.castEnergyBall.restoreCD.Timer = value.cebRestoreTimer;
        }
    }

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadChildComponent(ref this.groundCol, transform.Find("Ground"), "LoadGroundCol()");
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.LoadComponent(ref this.playerAnimator, transform.Find("Model"), "LoadAnimator()");
        this.LoadComponent(ref this.shootPoint, transform.Find("ShootPoint"), "LoadShootPoint()");
        this.LoadComponent(ref this.dashTrail, transform.Find("DashEffect"), "LoadDashTrail()");
        this.DefaultStat();
    }

    protected virtual void Update()
    {
        this.GroundChecking();
        this.InteractChecking();
        this.Moving();
        this.Jumping();
        this.Facing();
        if (this.hasDash) this.Dashing();
        if (this.hasAirJump) this.AirJumping();
        if (this.hasCastEnergyBall) this.CastingEnergyBall();
        this.playerAnimator.HandlingAnimator(this);
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
        UtilManager.Instance.CheckIsGround(this.groundCol, this.groundLayer, this.groundTag, ref this.prevIsGround, ref this.isGround);
    }

    //=======================================Interact Check=======================================
    protected virtual void InteractChecking()
    {
        this.interactableObj = null;

        Vector2 start = transform.position;
        float xDir = Mathf.Cos(transform.rotation.eulerAngles.y * Mathf.Deg2Rad);
        Vector2 dir = new Vector2(xDir, 0);
        RaycastHit2D[] hits = Physics2D.RaycastAll(start, dir, this.interactDetectLength);
        Interactable interactable = null;

        foreach (RaycastHit2D hit in hits)
        {
            interactable = hit.transform.GetComponent<Interactable>();
            if (interactable != null)
            {
                this.interactableObj = hit.transform;
                break;
            }
        }

        if (interactable != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactable.Interact(this);
                return;
            }
        }

        Debug.DrawRay(start, dir * this.interactDetectLength, Color.red);
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
        if (!this.dash.isDashing && !this.castEnergyBall.isCasting)
        {
            this.moveDir = (int)InputManager.Instance.MoveDir.x;
            MovementManager.Instance.MoveWithAcceleration(this.rb, this.moveDir, this.moveSpeed, this.moveSpeedUpTime, this.moveSlowDownTime);
        }

        if (this.rb.velocity.x >= Mathf.Pow(1, 1) || this.rb.velocity.x <= -Mathf.Pow(1, 1)) this.isMoving = true;
        else this.isMoving = false;
    }

    protected virtual void FinishMove()
    {
        MovementManager.Instance.StopMove(this.rb);
        this.isMoving = false;
    }

    //============================================Jump============================================
    protected virtual void Jumping()
    {
        if (this.isGround && !this.isJumping) this.jumpStartCD.ResetStatus();

        if (!this.dash.isDashing)
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
                    if (this.rb.velocity.y <= Mathf.Pow(0.1f, 3)) this.isJumping = false;
                }
            }
        }
    }

    protected virtual void FinishJump()
    {
        MovementManager.Instance.StopJump(this.rb);
        this.isJumping = false;
    }

    //============================================Face============================================
    protected virtual void Facing()
    {
        if (this.dash.isDashing) return;
        if (this.moveDir == 0) return;
        UtilManager.Instance.RotateFaceDir(this.moveDir, this.transform);
    }

    //============================================Dash============================================
    protected virtual void Dashing()
    {
        SkillManager.Instance.Dash.Update(this);
    }

    protected virtual void FinishDash()
    {
        SkillManager.Instance.Dash.FinishDash(this);
    }

    //==========================================Air Jump==========================================
    protected virtual void AirJumping()
    {
        SkillManager.Instance.AirJump.Update(this);
    }

    protected virtual void FinishAirJump()
    {
        SkillManager.Instance.AirJump.FinishAirJump(this);
    }

    //======================================Cast Energy Ball======================================
    protected virtual void CastingEnergyBall()
    {
        SkillManager.Instance.CastEBall.Update(this);
    }

    protected virtual void FinishCastEnergyBall()
    {
        SkillManager.Instance.CastEBall.Finish(this);
    }

    //===========================================Other============================================
    public virtual void DefaultStat()
    {
        if (this.so == null)
        {
            Debug.LogError("No PlayerSO", transform.gameObject);
            return;
        }

        // Entity.Stat
        this.maxHealth = this.so.maxHealth;
        this.health = this.maxHealth;

        // Move
        this.moveSpeed = this.so.moveSpeed;
        this.moveSpeedUpTime = this.so.moveSpeedUpTime;
        this.moveSlowDownTime = this.so.moveSlowDownTime;

        // Jump
        this.jumpSpeed = this.so.jumpSpeed;
        this.jumpStartCD = new Cooldown(this.so.jumpStartDuration, 0);

        // Dash
        this.dash = new DashData();
        this.dash.speed = this.so.dashSpeed;
        this.dash.restoreCD = new Cooldown(this.so.dashRestoreDuration, 0);
        this.dash.dashCD = new Cooldown(this.so.dashDuration, 0);

        // AirJump
        this.airJump = new AirJumpData();
        this.airJump.jumpSpeed = this.so.airJumpSpeed;
        this.airJump.jumpStartCD = new Cooldown(this.so.airJumpStartDuration, 0);

        // Cast Energy Ball
        this.castEnergyBall = new CastEnergyBallData();
        this.castEnergyBall.restoreCD = new Cooldown(this.so.cebRestoreDuration, 0);
        this.castEnergyBall.chargeCD = new Cooldown(this.so.cebChargeDuration, 0);
        this.castEnergyBall.endCD = new Cooldown(this.so.cebEndDuration, 0);
    }


    //============================================================================================
    //=========================================Interface==========================================
    //============================================================================================

    //=========================================Dash Skill=========================================
    // ===Property===
    Rigidbody2D IDash.GetRb()
    {
        return this.rb;
    }

    int IDash.GetMoveDir()
    {
        this.moveDir = (int)InputManager.Instance.MoveDir.x;
        if (this.moveDir != 0) return this.moveDir;
        if (transform.eulerAngles.y == 0) return 1;
        else return -1;
    }

    ref int IDash.GetDashDir()
    {
        return ref this.dash.dir;
    }

    float IDash.GetDashSpeed()
    {
        return this.dash.speed;
    }

    Cooldown IDash.GetSkillCD()
    {
        return this.dash.restoreCD;
    }

    Cooldown IDash.GetDashCD()
    {
        return this.dash.dashCD;
    }

    ref bool IDash.GetIsDashing()
    {
        return ref this.dash.isDashing;
    }

    // ===Condition===
    bool IDash.CanRechargeSkill()
    {
        if (this.dash.restoreCD.Timer > 0) return true; // already in recharging
        if (this.isGround) return true; // is ground
        return false;
    }

    bool IDash.CanDash()
    {
        if (InputManager.Instance.ShiftState == 0) return false; // not press or hold shift
        if (this.castEnergyBall.isCasting) return false; // is casting energy ball
        return true;
    }

    // ===Additional===
    void IDash.Enter()
    {
        this.dashTrail.emitting = true;
        if (this.isJumping) this.FinishJump();
        if (this.airJump.isJumping) this.FinishAirJump();
        if (this.isMoving) this.FinishMove();
    }

    void IDash.Exit()
    {
        this.dashTrail.emitting = false;
    }

    //=======================================Air Jump Skill=======================================
    // ===Property===
    Rigidbody2D IAirJump.GetRb()
    {
        return this.rb;
    }

    float IAirJump.GetJumpSpeed()
    {
        return this.airJump.jumpSpeed;
    }

    ref bool IAirJump.GetIsAirJumping()
    {
        return ref this.airJump.isJumping;
    }

    ref bool IAirJump.GetIsUsed()
    {
        return ref this.airJump.isJump;
    }

    // ===Condition===
    bool IAirJump.CanRestore()
    {
        return this.isGround;
    }

    bool IAirJump.CanJump()
    {
        if (InputManager.Instance.SpaceState != 1) return false; // not press or hold space
        if (this.isGround) return false; // is ground
        if (this.isJumping) return false; // is jumping
        if (this.castEnergyBall.isCasting) return false; // is casting energy ball
        return true;
    }

    // ===Addition===
    void IAirJump.Enter()
    {
        if (this.dash.isDashing) this.FinishDash();
    }

    //======================================Cast Energy Ball======================================
    // ===Property===
    Rigidbody2D ICastEnergyBall.GetRb()
    {
        return this.rb;
    }

    Cooldown ICastEnergyBall.GetSkillCD()
    {
        return this.castEnergyBall.restoreCD;
    }

    Cooldown ICastEnergyBall.GetChargeCD()
    {
        return this.castEnergyBall.chargeCD;
    }

    Cooldown ICastEnergyBall.GetEndCD()
    {
        return this.castEnergyBall.endCD;
    }

    int ICastEnergyBall.GetDir()
    {
        if (transform.eulerAngles.y == 180f) return -1;
        else return 1;
    }

    Vector2 ICastEnergyBall.GetBallSpawnPos()
    {
        return this.shootPoint.position;
    }

    ref bool ICastEnergyBall.GetIsCharging()
    {
        return ref this.castEnergyBall.isCharging;
    }

    ref bool ICastEnergyBall.GetIsFinishing()
    {
        return ref this.castEnergyBall.isFinishing;
    }

    ref bool ICastEnergyBall.GetIsCasting()
    {
        return ref this.castEnergyBall.isCasting;
    }

    // ===Condition===
    bool ICastEnergyBall.CanCastEnergyBall()
    {
        if (!Input.GetKeyDown(KeyCode.J)) return false;
        return true;
    }

    // ===Addition===
    void ICastEnergyBall.Enter()
    {
        if (this.dash.isDashing) this.FinishDash();
        if (this.isJumping) this.FinishJump();
        if (this.airJump.isJumping) this.FinishAirJump();
    }

    //=========================================Damagable==========================================
    void Damagable.TakeDamage(int damage)
    {
        if (this.health <= 0) return;
        this.health -= damage;
        
        if (this.health <= 0)
        {
            this.health = 0;
            EventManager.Instance.OnPlayerDead?.Invoke();
        }
    }

    //====================================Controllable By Door====================================
    void ControllableByDoor.Move(int dir)
    {
        MovementManager.Instance.MoveWithAcceleration(this.rb, dir, this.moveSpeed, this.moveSpeedUpTime, this.moveSlowDownTime);
    }

    float ControllableByDoor.GetXPos()
    {
        return transform.position.x;
    }

    //=========================================Door User==========================================
    Transform DoorUser.GetTrans()
    {
        return transform;
    }
}
