using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : Entity, Damagable, DoorUser, BonfireUser, SpikeUser, TeleDoorUser, ItemUser
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
    [SerializeField] protected TrailRenderer dashTrail;
    [SerializeField] protected Katana katana;

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

    [Header("Invincible")]
    [SerializeField] protected Cooldown invincibleCD;
    [SerializeField] protected bool isInvincible;

    [Space(25)]

    [Header("Move")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveSpeedUpTime;
    [SerializeField] protected float moveSlowDownTime;
    [SerializeField] protected int moveDir;
    [SerializeField] protected bool isMoving;

    [Space(25)]

    [Header("Face")]
    [SerializeField] protected int faceDir;

    [Space(25)]

    [Header("Appear")]
    [SerializeField] protected Cooldown appearCD;
    [SerializeField] protected bool isAppearing = true;

    [Space(25)]

    [Header("Jump")]
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected Cooldown jumpStartCD;
    [SerializeField] protected bool isJumping;
    [SerializeField] protected bool isJump;

    [Space(25)]

    [Header("Dash")]
    [SerializeField] protected DashData dash;
    // Support
    [SerializeField] protected float dashTrailDistance;

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

    // ===Stat===
    public bool HasDash { get => this.hasDash; set => this.hasDash = value; }
    public bool HasAirJump { get => this.hasAirJump; set => this.hasAirJump = value; }
    public bool HasCastEnergyBall { get => this.hasCastEnergyBall; set => this.hasCastEnergyBall = value; }

    // ===Ground Check===
    public bool PrevIsGround => this.prevIsGround;
    public bool IsGround => this.isGround;

    // ===Interact Check===
    public Transform InteractableObj => this.interactableObj;

    // ===Invincible===
    public Cooldown InvincibleCD => this.invincibleCD;
    public bool IsInvincible => this.isInvincible;

    // ===Despawn By Time===
    public Cooldown DespawnCD => this.despawnCD;

    // ===Move===
    public float MoveSpeed => this.moveSpeed;
    public bool IsMoving => this.isMoving;

    // ===Appear===
    public Cooldown AppearCD => this.appearCD;
    public bool IsAppearing => this.isAppearing;

    // ===Jump===
    public bool IsJumping => this.isJumping;

    // ===Dash Skill===
    public bool IsDashing => this.dash.isDashing;

    // ===Air Jump Skill===
    public bool IsAirJumping => this.airJump.isJumping;
    public bool IsAirJump => this.airJump.isJump;

    // ===Cast Energy Ball===
    public bool IsCastingEnergyBall => this.castEnergyBall.isCasting;
    public bool IsChargingEnergyBall => this.castEnergyBall.isCharging;
    public bool IsShootingEnergyBall => this.castEnergyBall.isShooting;

    // ===Slash===
    public bool isSlashing => this.katana.IsAttacking;
    
    // ===Db===
    public PlayerDbData Db
    {
        get
        {
            GameManager gameManager = GameManager.Instance;
            return new PlayerDbData(this.dbId, gameManager.AccountId, gameManager.RespawnSceneIndex, gameManager.RespawnPos,
                gameManager.RespawnRot.eulerAngles, gameManager.CurrSceneIndex, transform.position, transform.rotation, this.id, this.health, 
                this.dash.dashCD.Timer, this.castEnergyBall.restoreCD.Timer, this.hasDash, this.hasAirJump, 
                this.hasCastEnergyBall);
        }
        set
        {
            this.dbId = value.DbId;
            transform.position = new Vector3(value.XPos, value.YPos, value.ZPos);
            transform.rotation = Quaternion.Euler(value.XRot, value.YRot, value.ZRot);
            this.id = value.Id;
            GameManager.Instance.RespawnSceneIndex = value.RespawnSceneIndex;
            GameManager.Instance.RespawnPos = new Vector3(value.RespawnXPos, value.RespawnYPos, value.RespawnZPos);
            GameManager.Instance.RespawnRot = Quaternion.Euler(value.RespawnXRot, value.RespawnYRot, value.RespawnZRot);
            GameManager.Instance.CurrSceneIndex = value.CurrSceneIndex;
            this.health = value.Health;
            this.dash.dashCD.Timer = value.DashRestoreTimer;
            this.castEnergyBall.restoreCD.Timer = value.CebRestoreTimer;
            this.hasDash = value.HasDash;
            this.hasAirJump = value.HasAirJump;
            this.hasCastEnergyBall = value.HasCastEnergyBall;
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
        this.LoadComponent(ref this.katana, transform.Find("Katana"), "LoadKatana()");
        this.DefaultStat();
    }

    protected override void Awake()
    {
        base.Awake();
        this.Register();
    }

    protected override void Update()
    {
        base.Update();

        if (this.health > 0 && !this.isAppearing)
        {
            this.CheckingGround();
            this.CheckingInteract();
            this.Moving();
            this.Jumping();
            this.Facing();
            this.HandlingInvincible();
            this.HandlingKatana();
            if (this.hasDash) this.Dashing();
            if (this.hasAirJump) this.AirJumping();
            if (this.hasCastEnergyBall) this.CastingEnergyBall();
        }

        else
        {
            this.rb.velocity = Vector2.zero;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EventManager.Instance.OnBonfireStopResting?.Invoke();
            }
        }

        this.playerAnimator.HandlingAnimator();
        this.Appearing();
        this.Despawning();
    }

    protected virtual void FixedUpdate()
    {
        if (this.isAppearing)
        {
            this.rb.velocity = Vector2.zero;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.Instance.OnPlayerAppear?.Invoke();
        this.isAppearing = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if (item != null) item.PickedUp(this);

        TeleDoor teleDoor = collision.GetComponent<TeleDoor>();
        if (teleDoor != null) teleDoor.Teleport(this);

        JumpPad jumpPad = collision?.transform?.parent?.GetComponent<JumpPad>();
        if (jumpPad != null) jumpPad.Push(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Spike spike = collision.transform.GetComponent<Spike>();
        if (spike != null) spike.Collide(this);
    }



    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //=======================================Event Register=======================================
    protected virtual void Register()
    {
        EventManager.Instance.OnQuit += this.Save;
    }

    protected virtual void Save()
    {
        DataBaseManager.Instance.Player.Update(this.Db);
    }

    //========================================Ground Check========================================
    protected virtual void CheckingGround()
    {
        Util.Instance.CheckIsGround(this.groundCol, this.groundLayer, this.groundTag, ref this.prevIsGround, 
            ref this.isGround);
    }

    //=======================================Interact Check=======================================
    protected virtual void CheckingInteract()
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
            interactable.Detected(this);
            if (InputManager.Instance.InteractState == 1)
            {
                interactable.Interact(this);
                return;
            }
        }

        Debug.DrawRay(start, dir * this.interactDetectLength, Color.red);
    }

    //========================================Dash Effect=========================================
    protected virtual void HandlingDashEffect()
    {
        float yPos = this.dashTrail.transform.localPosition.y;
        float zPos = this.dashTrail.transform.localPosition.z;
        this.dashTrail.transform.localPosition = new Vector3(this.dashTrailDistance, yPos, zPos);
    }

    //=========================================Invincible=========================================
    protected virtual void HandlingInvincible()
    {
        if (!this.isInvincible) return;
        this.invincibleCD.CoolingDown();

        if (!this.invincibleCD.IsReady) return;
        this.isInvincible = false;
        this.invincibleCD.ResetStatus();
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    //======================================Despawn By Time=======================================
    protected override void Despawning()
    {
        if (this.health > 0) return;
        this.despawnCD.CoolingDown();

        if (!this.despawnCD.IsReady) return;
        this.despawnCD.ResetStatus();
        EventManager.Instance.OnPlayerDead?.Invoke();
    }

    //============================================Move============================================
    public virtual void Moving()
    {
        if (!this.dash.isDashing && !this.castEnergyBall.isCasting)
        {
            this.moveDir = InputManager.Instance.MoveDir.x > 0 ? 1 : InputManager.Instance.MoveDir.x < 0 ? -1 : 0;
            Util.Instance.MovingWithAccelerationInHorizontal(this.rb, this.moveDir, this.moveSpeed, 
                this.moveSpeedUpTime, this.moveSlowDownTime);
        }

        if (this.rb.velocity.x >= Mathf.Pow(1, 1) || this.rb.velocity.x <= -Mathf.Pow(1, 1)) this.isMoving = true;
        else this.isMoving = false;
    }

    protected virtual void FinishMove()
    {
        Util.Instance.StopMove(this.rb);
        this.isMoving = false;
    }

    //============================================Jump============================================
    protected virtual void Jumping()
    {
        if (this.isGround && !this.isJumping) this.jumpStartCD.ResetStatus();

        if (!this.dash.isDashing)
        {
            bool isSpacePressed = InputManager.Instance.JumpState != 0;
            Util movment = Util.Instance;

            if (this.isGround && isSpacePressed && !this.isJumping)
            {
                this.isJump = true;
                this.isJumping = true;
                movment.Jump(this.rb, this.jumpSpeed);
            }

            else if (this.isJumping)
            {
                if (!this.jumpStartCD.IsReady) this.jumpStartCD.CoolingDown();
                else if (this.rb.velocity.y <= Mathf.Pow(0.1f, 3)
                    || !isSpacePressed) this.FinishJump();
            }

            if (this.isGround && this.isJump && !this.isJumping)
            {
                this.isJump = false;
            }
        }
    }

    protected virtual void FinishJump()
    {
        Util.Instance.StopJump(this.rb);
        this.isJumping = false;
    }

    //============================================Face============================================
    protected virtual void Facing()
    {
        if (this.dash.isDashing || this.moveDir == 0 || this.katana.IsAttacking) return;
        Util.Instance.RotateFaceDir(this.moveDir, this.transform);

        if (this.moveDir >= 1) this.faceDir = 1;
        else if (this.moveDir <= -1) this.faceDir = -1;
    }

    //===========================================Katana===========================================
    protected virtual void HandlingKatana()
    {
        this.katana.MyUpdate();
        if (InputManager.Instance.AttackState != 0) this.katana.Attack();
    }

    //===========================================Appear===========================================
    protected virtual void Appearing()
    {
        if (!this.isAppearing) return;
        this.appearCD.CoolingDown();
        this.rb.velocity = Vector2.zero;

        if (!this.appearCD.IsReady) return;
        this.appearCD.ResetStatus();
        this.isAppearing = false;
    }

    //============================================Dash============================================
    protected virtual void Dashing()
    {
        if (!this.hasDash || this.castEnergyBall.isCasting) return;

        else if (!this.dash.isDashing && !this.dash.restoreCD.IsReady && (this.dash.restoreCD.Timer > 0 
            || this.isGround))
        {
            this.RechargeDash();
        }

        else if (this.dash.isDashing)
        {
            this.OnDash();
        }

        else if (this.dash.restoreCD.IsReady && (InputManager.Instance.DashState != 0))
        {
            this.StartDash();
        }
    }

    protected virtual void RechargeDash()
    {
        this.dash.restoreCD.CoolingDown();
    }

    protected virtual void StartDash()
    {
        if (this.isJumping) this.FinishJump();
        if (this.airJump.isJumping) this.FinishAirJump();
        if (this.isMoving) this.FinishMove();

        if (this.moveDir != 0) this.dash.dir = this.moveDir;
        else this.dash.dir = this.faceDir;
        this.dash.isDashing = true;
        this.dash.restoreCD.ResetStatus();
        this.dashTrail.emitting = true;
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        this.rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
    }

    protected virtual void OnDash()
    {
        this.rb.velocity = new Vector2(this.dash.speed * this.dash.dir, 0);
        this.dash.dashCD.CoolingDown();
        this.rb.WakeUp();

        if (!this.dash.dashCD.IsReady) return;
        this.FinishDash();
    }

    protected virtual void FinishDash()
    {
        this.dashTrail.emitting = false;
        this.dash.isDashing = false;
        this.dash.dashCD.ResetStatus();
        this.rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        gameObject.layer = LayerMask.NameToLayer("Default");
        Util.Instance.StopMove(this.rb);
    }

    //==========================================Air Jump==========================================
    protected virtual void AirJumping()
    {
        if (!this.hasAirJump && this.castEnergyBall.isCasting) return;

        else if (this.isGround && !this.airJump.isJumping && this.airJump.isJump)
        {
            this.RestoreAirJump();
        }

        else if (InputManager.Instance.JumpState == 1 && !this.airJump.isJumping
            && !this.isJumping && !this.airJump.isJump)
        {
            this.StartAirJump();
        }

        else if (this.airJump.isJumping && this.rb.velocity.y <= Mathf.Pow(0.1f, 3))
        {
            this.FinishAirJump();
        }
    }

    protected virtual void StartAirJump()
    {
        if (this.dash.isDashing) this.FinishDash();
        Util.Instance.Jump(this.rb, this.airJump.jumpSpeed);
        this.airJump.isJumping = true;
        this.airJump.isJump = true;
    }

    protected virtual void RestoreAirJump()
    {
        this.airJump.isJump = false;
    }

    protected virtual void FinishAirJump()
    {
        this.airJump.isJumping = false;
    }

    //======================================Cast Energy Ball======================================
    protected virtual void CastingEnergyBall()
    {
        if (!this.hasCastEnergyBall) return;
        else if (!this.castEnergyBall.isCasting && !this.castEnergyBall.restoreCD.IsReady)
        {
            this.RechargeCastEnergyBall();
        }

        else if (Input.GetKeyDown(KeyCode.I) && this.castEnergyBall.restoreCD.IsReady)
        {
            this.ActivateCastEnergyBall();
        }

        else if (this.castEnergyBall.isCasting)
        {
            this.CastEnergyBall();
        }
    }

    protected virtual void RechargeCastEnergyBall()
    {
        this.castEnergyBall.restoreCD.CoolingDown();
    }

    protected virtual void ActivateCastEnergyBall()
    {
        if (this.dash.isDashing) this.FinishDash();
        if (this.isJumping) this.FinishJump();
        if (this.airJump.isJumping) this.FinishAirJump();

        this.castEnergyBall.isCasting = true;
        this.castEnergyBall.isCharging = true;
        this.rb.velocity = Vector2.zero;
        this.rb.constraints |= RigidbodyConstraints2D.FreezePositionY;
        this.rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
        this.rb.WakeUp();
    }

    protected virtual void CastEnergyBall()
    {
        if (this.castEnergyBall.isCharging)
        {
            this.ChargeEnergyBall();    
        }

        else if (this.castEnergyBall.isShooting)
        {
            this.AfterShootEnergyBall();
        }
    }

    protected virtual void ChargeEnergyBall()
    {
        this.castEnergyBall.chargeCD.CoolingDown();

        if (!this.castEnergyBall.chargeCD.IsReady) return;
        this.castEnergyBall.chargeCD.ResetStatus();
        this.castEnergyBall.isCharging = false;
        this.castEnergyBall.isShooting = true;
        this.ShootEnergyBall();
    }

    protected virtual void ShootEnergyBall()
    {
        float angle = 0;

        if (this.faceDir == -1) angle = 180;
        Vector2 spawnPos = this.shootPoint.position;
        Quaternion spawnRot = Quaternion.Euler(0, 0, angle);

        Transform newEBall = BulletSpawner.Instance.SpawnByName("EnergyBall", spawnPos, spawnRot);
        newEBall.gameObject.SetActive(true);
    }

    protected virtual void AfterShootEnergyBall()
    {
        this.castEnergyBall.shootCD.CoolingDown();

        if (!this.castEnergyBall.shootCD.IsReady) return;
        this.FinishCastEnergyBall();
    }

    protected virtual void FinishCastEnergyBall()
    {
        this.castEnergyBall.isCasting = false;
        this.castEnergyBall.isShooting = false;
        this.castEnergyBall.shootCD.ResetStatus();
        this.rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        this.rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        this.rb.WakeUp();
    }

    protected virtual void FinishImediatelyCastEnergyBall()
    {
        this.castEnergyBall.isCasting = false;
        this.castEnergyBall.isCharging = false;
        this.castEnergyBall.isShooting = false;
        this.castEnergyBall.shootCD.ResetStatus();
        this.castEnergyBall.chargeCD.ResetStatus();
        if (this.rb.constraints == RigidbodyConstraints2D.FreezePositionY) 
            this.rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        if (this.rb.constraints == RigidbodyConstraints2D.FreezePositionX) 
            this.rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        this.rb.WakeUp();
    }

    //===========================================Other============================================
    public virtual void DefaultStat()
    {
        if (this.so == null)
        {
            Debug.LogError("No PlayerSO", transform.gameObject);
            return;
        }

        this.DefaultEntity(this.so);

        // Entity.Stat
        this.maxHealth = this.so.maxHealth;
        this.health = this.maxHealth;

        // Interact Check
        this.interactDetectLength = this.so.interactDetectLength;

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
        this.castEnergyBall.shootCD = new Cooldown(this.so.cebEndDuration, 0);
    }

    public void Revive()
    {
        this.health = this.maxHealth;
    }

    

    //============================================================================================
    //=========================================Interface==========================================
    //============================================================================================

    //=========================================Damagable==========================================
    void Damagable.TakeDamage(int damage, Transform attacker)
    {
        if (this.isInvincible || this.health <= 0) return;
        this.health -= damage;
        this.isInvincible = true;
        this.isHurting = true;
        gameObject.layer = LayerMask.NameToLayer("Invincible");

        if (this.health <= 0)
        {
            this.health = 0;
            gameObject.layer = LayerMask.NameToLayer("Dead");
        }
    }

    void Damagable.Push(Vector2 force)
    {
        if (this.isInvincible && this.invincibleCD.Timer > 0) return;
        this.rb.velocity = force;
    }

    //=========================================Door User==========================================
    void DoorUser.Move(int dir)
    {
        Util.Instance.MovingWithAccelerationInHorizontal(this.rb, dir, this.moveSpeed, this.moveSpeedUpTime, 
            this.moveSlowDownTime);
    }

    float DoorUser.GetXPos()
    {
        return transform.position.x;
    }

    Transform DoorUser.GetTrans()
    {
        return transform;
    }

    //========================================Bonfire User========================================
    Vector2 BonfireUser.GetPos()
    {
        return transform.position;
    }

    void BonfireUser.Rest()
    {
        this.health = this.maxHealth;
        this.FinishMove();
    }

    //===========================================ISpike===========================================
    Transform SpikeUser.GetTarget()
    {
        return transform;
    }

    Damagable SpikeUser.GetDamagable()
    {
        return this;
    }

    //=========================================Item User==========================================
    void ItemUser.AddHealth(int restoredHealth)
    {
        this.health += restoredHealth;
        if (this.health > this.maxHealth) this.health = this.maxHealth;
    }

    void ItemUser.UnlockSkill(SkillType unlockedSkill)
    {
        if (unlockedSkill == SkillType.DASH)
        {
            this.hasDash = true;
            EventManager.Instance.OnPlayerGetDash?.Invoke();
        }
        else if (unlockedSkill == SkillType.AIR_JUMP)
        {
            this.hasAirJump = true;
            EventManager.Instance.OnPlayerGetAirJump?.Invoke();
        }
        else if (unlockedSkill == SkillType.CAST_ENERGY_BALL)
        {
            this.hasCastEnergyBall = true;
            EventManager.Instance.OnPlayergetCastEnergyBall?.Invoke();
        }
    }

    void ItemUser.AddCoin(int add)
    {
        GameManager.Instance.CurrCoin += add;
    }

    //=======================================Tele Door User=======================================
    void TeleDoorUser.SetPos(Vector2 newPos)
    {
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
