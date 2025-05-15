using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class Reaper : Enemy
{
    public enum SlashState
    {
        NULL = 0,
        CHARGE = 1,
        ATTACK = 2,
        FINISH = 3,
    }

    public enum RiseHandState
    {
        NULL = 0,
        CHARGE = 1,
        ATTACK = 2,
    }

    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Reaper===")]
    [Header("Component")]
    [SerializeField] protected SpriteRenderer avatar;
    [SerializeField] protected ReaperAnimator animator;

    [Header("Check Ground")]
    [SerializeField] protected CapsuleCollider2D groundCol;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected string groundTag;
    [SerializeField] protected bool prevIsGround;
    [SerializeField] protected bool isGround;

    [Space(25)]

    [Header("Move")]
    [SerializeField] protected int moveDir;
    [SerializeField] protected float slowDownTime;
    [SerializeField] protected float speedUpTime;
    [SerializeField] protected bool isMoving;

    [Space(25)]

    [Header("Chase Target")]
    [SerializeField] protected Transform target;
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected float startChaseDistance;
    [SerializeField] protected float stopChaseDistance;

    [Space(25)]

    [Header("Attackable")]
    [SerializeField] protected LayerMask attackableLayer;
    [SerializeField] protected List<string> attackableTags = new List<string>();

    [Space(25)]

    [Header("Attack Transition")]
    [SerializeField] protected Cooldown attackCD;
    [SerializeField] protected bool isTransitioningAttack;

    [Space(25)]

    [Header("Slash")]
    [SerializeField] protected CircleCollider2D slashCol;
    [SerializeField] protected Transform slashShootPoint;
    [SerializeField] protected string slashName;
    [SerializeField] protected int slashDamage;
    [SerializeField] protected float slashForce;
    [SerializeField] protected float slashMaxRange;
    [SerializeField] protected SlashState currSlashState;
    [SerializeField] protected Cooldown slashRestoreCD;
    [SerializeField] protected Cooldown slashChargeCD;
    [SerializeField] protected Cooldown slashAttackCD;
    [SerializeField] protected Cooldown slashFinishCD;
    [SerializeField] protected bool isSlashing;

    [Space(25)]

    [Header("Rise Hand")]
    [SerializeField] protected string riseHandName;
    [SerializeField] protected float riseHandMinRange;
    [SerializeField] protected RiseHandState currRiseHandState;
    [SerializeField] protected Vector2 riseHandSpawnPosRange;
    [SerializeField] protected Cooldown restoreRiseHandCD;
    [SerializeField] protected Cooldown chargeRiseHandCD;
    [SerializeField] protected Cooldown riseHandAttackCD;
    [SerializeField] protected bool isRisingHand;

    [Space(25)]

    [Header("Following Hand")]
    [SerializeField] protected string followingHandName;
    [SerializeField] protected Vector2 followingHandSpawnPosRange;
    [SerializeField] protected Cooldown restoreFollowingHandCD;

    [Space(25)]

    [Header("Cast Ball")]
    [SerializeField] protected string ballName;
    [SerializeField] protected float castBallMinRange;
    [SerializeField] protected Transform castBallShootPoint;
    [SerializeField] protected Cooldown restoreCastBallCD;
    [SerializeField] protected Cooldown chargeCastBallCD;
    [SerializeField] protected bool isCastingBall;

    [Space(25)]

    [Header("Teleport")]
    [SerializeField] protected TeleLightning teleLightning;
    [SerializeField] protected List<Transform> telePoints;
    [SerializeField] protected int teleTimes;
    [SerializeField] protected int currTimes;
    [SerializeField] protected Cooldown restoreTeleCD;
    [SerializeField] protected Cooldown teleCD;
    [SerializeField] protected Cooldown teleAppearCD;
    [SerializeField] protected bool isTeleporting;
    [SerializeField] protected bool isTeleAppearing;



    //==========================================Get Set===========================================
    // Move
    public bool IsMoving => this.isMoving;
    
    // Chase Target
    public float ChaseSpeed => this.chaseSpeed;

    //Slash
    public SlashState CurrSlashState => this.currSlashState;
    public Cooldown SlashChargeCD => this.slashChargeCD;
    public Cooldown SlashAttackCD => this.slashAttackCD;
    public Cooldown SlashFinishCD => this.slashFinishCD;
    public bool IsSlashing => this.isSlashing;

    // Rise Hand
    public RiseHandState CurrRiseHandState => this.currRiseHandState;
    public Cooldown ChargeRiseHandCD => this.chargeRiseHandCD;
    public Cooldown RiseHandAttackCD => this.riseHandAttackCD;
    public bool IsRisingHand => this.isRisingHand;

    // Cast Ball
    public Cooldown CastBallCD => this.chargeCastBallCD;
    public bool IsCastingBall => this.isCastingBall;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.avatar, transform.Find("Model"), "LoadAvatar()");
        this.LoadComponent(ref this.animator, transform.Find("Model"), "LoadAnimator()");
        this.LoadComponent(ref this.groundCol, transform.Find("Ground"), "LoadGroundCol()");
        this.LoadComponent(ref this.slashCol, transform.Find("Slash"), "LoadSlashCol()");
        this.LoadComponent(ref this.slashShootPoint, transform.Find("SlashShootPoint"), "LoadSlashShootPoint()");
        this.LoadComponent(ref this.castBallShootPoint, transform.Find("CastBallShootPoint"), "LoadCastBallShootPoint()");
        this.LoadComponent(ref this.teleLightning, transform.Find("Lightning"), "LoadTeleLightning()");
    }

    protected override void Update()
    {
        base.Update();
        this.CheckingGround();
        this.TransitioningAttack();
        if (this.health > 0)
        {
            this.Moving();
            this.Facing();
            this.HandlingFollowingHand();
            this.HandlingTeleport();
            this.HandlingRisingHand();
            this.CastingBall();
            this.Slashing();
        }
        this.animator.HandlingAnimator();
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if (this.isTeleAppearing) return;
        base.OnCollisionStay2D(collision);
    }



    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //======================================Ground Checking=======================================
    protected virtual void CheckingGround()
    {
        Util.Instance.CheckIsGround(this.groundCol, this.groundLayer, this.groundTag, ref this.prevIsGround, ref this.isGround);
    }

    //===========================================Facing===========================================
    protected virtual void Facing()
    {
        if (this.moveDir == 0) return;
        Util.Instance.RotateFaceDir(this.moveDir, this.transform);
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
        if (this.rb.velocity.x >= Mathf.Pow(-0.1f, 3) && this.rb.velocity.x <= Mathf.Pow(0.1f, 3)) this.isMoving = false;
        else this.isMoving = true;
        this.ChasingTarget();
    }

    protected virtual void ChasingTarget()
    {
        if (this.target == null) return;
        this.moveDir = this.target.position.x - this.transform.position.x > 0 ? 1 : -1;
        float distance = Vector2.Distance(this.transform.position, this.target.position);

        if (distance > this.startChaseDistance || distance < this.stopChaseDistance || this.isRisingHand || this.isSlashing || this.isCastingBall)
            Util.Instance.SlowingDownWithAccelerationInHorizontal(this.rb, this.chaseSpeed, this.slowDownTime);
        else
            Util.Instance.MovingWithAccelerationInHorizontal(this.rb, this.moveDir, this.chaseSpeed, this.speedUpTime, this.slowDownTime);
    }

    //=====================================Attack Transition======================================
    protected virtual void TransitioningAttack()
    {
        if (!this.isTransitioningAttack) return;
        this.attackCD.CoolingDown();

        if (!this.attackCD.IsReady) return; 
        this.isTransitioningAttack = false;
        this.attackCD.ResetStatus();
    }

    protected virtual void OnFinishAnAttack()
    {
        this.isTransitioningAttack = true;
    }

    //===========================================Slash============================================
    protected virtual void Slashing()
    {
        this.RestoringSlash();
        if (this.isSlashing)
        {
            this.ChargingSlash();
            this.SlashAttacking();
            this.FinishingSlash();
        }

        if (!this.slashRestoreCD.IsReady || this.target == null || this.isCastingBall || this.isRisingHand
            || this.isTeleporting || this.isTransitioningAttack) return;
        float xDistance = Mathf.Abs(this.transform.position.x - this.target.position.x);
        
        if (xDistance > this.slashMaxRange) return;
        this.isSlashing = true;
        this.currSlashState = SlashState.CHARGE;
        this.slashRestoreCD.ResetStatus();
    }

    protected virtual void RestoringSlash()
    {
        if (this.isSlashing) return;
        this.slashRestoreCD.CoolingDown();
    }

    protected virtual void ChargingSlash()
    {
        if (this.currSlashState != SlashState.CHARGE) return;
        this.slashChargeCD.CoolingDown();

        if (!this.slashChargeCD.IsReady) return;
        this.currSlashState = SlashState.ATTACK;
        this.slashChargeCD.ResetStatus();
    }
    
    protected virtual void SlashAttacking()
    {
        if (this.currSlashState != SlashState.ATTACK) return;
        this.slashAttackCD.CoolingDown();
        this.CollidingSlash();

        if (!this.slashAttackCD.IsReady) return;
        this.currSlashState = SlashState.FINISH;
        this.slashAttackCD.ResetStatus();
        this.ShootSlashWave();
    }

    protected virtual void FinishingSlash()
    {
        if (this.currSlashState != SlashState.FINISH) return;
        this.slashFinishCD.CoolingDown();

        if (!this.slashFinishCD.IsReady) return;
        this.currSlashState = SlashState.NULL;
        this.slashFinishCD.ResetStatus();
        this.isSlashing = false;
        this.OnFinishAnAttack();
    }

    protected virtual void CollidingSlash()
    {
        Vector2 point = this.slashCol.transform.position;
        float rad = this.slashCol.radius;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, rad, this.attackableLayer);
        foreach (Collider2D collider in colliders)
        {
            if (this.attackableTags.Contains(collider.tag))
            {
                Damagable damagable = collider.GetComponent<Damagable>();
                
                if (damagable == null) return;
                damagable.TakeDamage(this.slashDamage);
                Vector2 dir = (collider.transform.position - this.transform.position).normalized;
                damagable.Push(this.slashForce * dir);
            }
        }
    }

    protected virtual void ShootSlashWave()
    {
        Vector2 spawnPos = this.slashShootPoint.position;
        Quaternion spawnRot = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y);
        Transform newBullet = BulletSpawner.Instance.SpawnByName(this.slashName, spawnPos, spawnRot);
        newBullet.gameObject.SetActive(true);
    }

    //========================================Rising Hand=========================================
    protected virtual void HandlingRisingHand()
    {
        this.RestoringRiseHand();
        if (this.isRisingHand)
        {
            this.ChargingRiseHand();
            this.RiseHandAttacking();
        }

        if (!this.restoreRiseHandCD.IsReady || this.target == null || this.isSlashing || this.IsCastingBall
            || this.isTeleporting || this.isTransitioningAttack) return;
        float distace = Vector2.Distance(this.transform.position, this.target.position);

        if (distace < this.slashMaxRange) return;
        this.isRisingHand = true;
        this.currRiseHandState = RiseHandState.CHARGE;
        this.restoreRiseHandCD.ResetStatus();
    }

    protected virtual void RestoringRiseHand()
    {
        if (this.isRisingHand) return;
        this.restoreRiseHandCD.CoolingDown();
    }

    protected virtual void ChargingRiseHand()
    {
        if (this.currRiseHandState != RiseHandState.CHARGE) return;
        this.chargeRiseHandCD.CoolingDown();

        if (!this.chargeRiseHandCD.IsReady) return;
        this.currRiseHandState = RiseHandState.ATTACK;
        this.chargeRiseHandCD.ResetStatus();
        Vector2 spawnPos = (Vector2)this.target.position + this.riseHandSpawnPosRange;
        Quaternion spawnRot = Quaternion.identity;
        Transform newRiseHand = BulletSpawner.Instance.SpawnByName(this.riseHandName, spawnPos, spawnRot);
        newRiseHand.gameObject.SetActive(true);
    }

    protected virtual void RiseHandAttacking()
    {
        if (this.currRiseHandState != RiseHandState.ATTACK) return;
        this.RiseHandAttackCD.CoolingDown();

        if (!this.RiseHandAttackCD.IsReady) return;
        this.currRiseHandState = RiseHandState.NULL;
        this.RiseHandAttackCD.ResetStatus();
        this.isRisingHand = false;
        this.OnFinishAnAttack();
    }

    //=======================================Following Hand=======================================
    protected virtual void HandlingFollowingHand()
    {
        this.restoreFollowingHandCD.CoolingDown();

        if (!this.restoreFollowingHandCD.IsReady) return;
        Vector2 spawnPos = (Vector2)this.target.position + this.followingHandSpawnPosRange;
        Quaternion spawnRot = Quaternion.Euler(0, 0, 90f);
        Transform newFollowingHand = BulletSpawner.Instance.SpawnByName(this.followingHandName, spawnPos, spawnRot);
        newFollowingHand.gameObject.SetActive(true);
        this.restoreFollowingHandCD.ResetStatus();
    }

    //=========================================Cast Ball==========================================
    protected virtual void CastingBall()
    {
        this.RestoringCastBall();
        this.ChargingCastBall();

        if (!this.restoreCastBallCD.IsReady || this.target == null || this.isSlashing 
            || this.isRisingHand|| this.isTransitioningAttack) return;
        float distance = Vector2.Distance(this.transform.position, this.target.position);

        if (distance < this.castBallMinRange) return;
        this.isCastingBall = true;
        this.restoreCastBallCD.ResetStatus();
    }

    protected virtual void RestoringCastBall()
    {
        if (this.isCastingBall) return;
        this.restoreCastBallCD.CoolingDown();

        if (!this.restoreCastBallCD.IsReady) return;
        this.isCastingBall = true;
        this.restoreCastBallCD.ResetStatus();
    }

    protected virtual void ChargingCastBall()
    {
        if (!this.isCastingBall) return;
        this.chargeCastBallCD.CoolingDown();

        if (!this.chargeCastBallCD.IsReady) return;
        this.isCastingBall = false;
        this.chargeCastBallCD.ResetStatus();
        this.ShootBall();
        this.OnFinishAnAttack();
    }

    protected virtual void ShootBall()
    {
        Vector2 spawnPos = this.castBallShootPoint.position;
        float distance = Vector2.Distance(this.transform.position, this.target.position);
        float angle = Mathf.Atan2(this.target.position.y - this.transform.position.y, this.target.position.x - this.transform.position.x) * Mathf.Rad2Deg;
        Quaternion spawnRot = Quaternion.Euler(0, 0, angle);
        Transform newBullet = BulletSpawner.Instance.SpawnByName(this.ballName, spawnPos, spawnRot);
        newBullet.gameObject.SetActive(true);
    }

    //==========================================Teleport==========================================
    protected virtual void HandlingTeleport()
    {
        this.RestoringTeleport();
        this.Teleporting();
        this.TeleAppearing();

        if (this.target == null || !this.restoreTeleCD.IsReady || this.isSlashing 
            || this.isCastingBall || this.isRisingHand || this.isTransitioningAttack) return;
        this.isTeleporting = true;
        this.restoreTeleCD.ResetStatus();
    }

    protected virtual void RestoringTeleport()
    {
        if (this.isTeleporting) return;
        this.restoreTeleCD.CoolingDown();
    }
    
    protected virtual void Teleporting()
    {
        if (!this.isTeleporting || this.isTeleAppearing) return;
        this.teleCD.CoolingDown();

        if (!this.teleCD.IsReady || this.isSlashing || this.isCastingBall || this.isRisingHand 
            || this.isTransitioningAttack) return;
        this.isTeleAppearing = true;
        this.teleCD.ResetStatus();
        this.teleLightning.CallDisappear();
        this.avatar.gameObject.SetActive(false);
        this.rb.gravityScale = 0;
        this.bodyCol.isTrigger = true;
        this.rb.velocity = Vector2.zero;
    }

    protected virtual void TeleAppearing()
    {
        if (!this.isTeleAppearing) return;
        this.teleAppearCD.CoolingDown();

        if (!this.teleAppearCD.IsReady) return;
        this.teleAppearCD.ResetStatus();
        this.isTeleAppearing = false;
        this.Teleport();
        this.currTimes++;

        if (this.currTimes < this.teleTimes) return;
        this.currTimes = 0;
        this.isTeleporting = false;
        this.OnFinishAnAttack();
    }

    protected virtual void Teleport()
    {
        float maxDistance = 0;
        Transform chosenPoint = null;
        foreach (Transform telePoint in this.telePoints)
        {
            float distance = Vector2.Distance(this.target.position, telePoint.position);
            if (distance < maxDistance) continue;
            maxDistance = distance;
            chosenPoint = telePoint;
        }

        this.avatar.gameObject.SetActive(true);
        this.rb.gravityScale = 1;
        this.transform.position = chosenPoint.position;
        this.teleLightning.CallAppear();
        this.bodyCol.isTrigger = false;
    }

    protected override void PlayDamageEffect(Vector2 dir)
    {
        if (this.isTeleAppearing) return;
        base.PlayDamageEffect(dir);
    }
}
