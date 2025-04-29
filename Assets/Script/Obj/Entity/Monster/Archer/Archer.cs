using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Archer : Monster
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Archer===")]
    [Header("Component")]
    [SerializeField] protected ArcherAnimator animator;
    [SerializeField] protected CapsuleCollider2D groundCol;

    [Space(25)]

    [Header("Ground Check")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected string groundTag = "Terrain";
    [SerializeField] protected bool prevIsGround;
    [SerializeField] protected bool isGround;

    [Space(25)]

    [Header("Wall Detection")]
    [SerializeField] protected float wallDetectDistance;
    [SerializeField] protected bool isWallDetected;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] protected string wallTag = "Terrain";

    [Space(25)]

    [Header("Target Detection")]
    [SerializeField] protected float targetDetectDistance;

    [Space(25)]

    [Header("Move")]
    [SerializeField] protected int moveDir;
    [SerializeField] protected float slowDownTime;
    [SerializeField] protected float speedUpTime;

    [Space(25)]

    [Header("Jump")]
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected bool isJumping;

    [Space(25)]

    [Header("Bow Attack")]
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected string arrowName;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected Cooldown bowRestoreCD;
    [SerializeField] protected Cooldown chargeBowCD;
    [SerializeField] protected bool isChargingBow;

    //==========================================Get Set===========================================    
    // ===Bow Attack===
    public Cooldown ChargBowCD => this.chargeBowCD;
    public bool IsChargingBow => this.isChargingBow;



    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform.Find("Model"), "LoadModel()");
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadCol()");
        this.LoadComponent(ref this.groundCol, transform.Find("Ground"), "LoadGroundCol()");
        this.LoadComponent(ref this.damageEff, transform.Find("DamageEffect"), "LoadDamageEff()");
        this.LoadComponent(ref this.shootPoint, transform.Find("ShootPoint"), "LoadShootPoint()");
    }

    protected virtual void Update()
    {
        this.DetectingWall();
        this.CheckingIsGround();
        this.DetectingTarget();
        this.CheckingTargetOutOfRange();
        this.Facing();
        this.Moving();
        //this.Jumping();
        this.UsingBow();
        this.animator.HandlingAnimator();
    }



    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //======================================Ground Checking=======================================
    protected virtual void CheckingIsGround()
    {
        Util.Instance.CheckIsGround(this.groundCol, this.groundLayer, this.groundTag, ref this.prevIsGround, ref this.isGround);
    }

    //=======================================Wall Detection=======================================
    protected virtual void DetectingWall()
    {
        float xDir = Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);
        float yDir = 0;
        Vector2 rayStart = transform.position + new Vector3(0, -0.5f, 0);
        Vector2 rayDir = new Vector2(xDir, yDir);

        Transform target = Util.Instance.ShootRaycast(this.wallDetectDistance, this.wallLayer, this.wallTag, rayStart, rayDir);
        this.isWallDetected = (target != null);
        Debug.DrawRay(rayStart, rayDir * wallDetectDistance, this.isWallDetected ? Color.green : Color.red);
    }

    //======================================Target Detection======================================
    protected override void DetectingTarget()
    {
        if (this.target != null) return;
        float xDir = Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);
        float yDir = 0;
        Vector2 rayStart = transform.position;
        Vector2 rayDir = new Vector2(xDir, yDir);

        this.target = Util.Instance.ShootRaycast(this.targetDetectDistance, this.targetLayer, this.targetTag, rayStart, rayDir);
        Debug.DrawRay(rayStart, rayDir * this.targetDetectDistance, this.target != null ? Color.green : Color.red);
    }

    //=======================================Face Direction=======================================
    protected override void Facing()
    {
        if (this.moveDir == 0) return;
        Util.Instance.RotateFaceDir(this.moveDir, this.transform);
    }

    //============================================Move============================================
    protected override void Moving() 
    {
        this.isMovingRandomly = false;
        this.isChasingTarget = false;

        if (this.target == null) this.MoveRandomly();
        else this.ChaseTarget();
    }

    // ===Move Randomly===
    protected virtual void MoveRandomly()
    {
        if (this.IsReachedEndPoint())
        {
            if (this.currEndPoint + 1 == this.endPoints.Count) this.currEndPoint = 0;
            else this.currEndPoint++;
        }

        this.moveDir = this.endPoints[this.currEndPoint].position.x > transform.position.x ? 1 : -1;
        Util.Instance.MoveWithAcceleration(this.rb, this.moveDir, this.slowSpeed, this.speedUpTime, this.slowDownTime);
        this.isMovingRandomly = true;
    }

    // ===Chase Target===
    protected virtual void ChaseTarget()
    {
        float currDistance = Vector2.Distance(this.target.position, transform.position);
        this.moveDir = this.target.position.x > transform.position.x ? 1 : -1;

        if (currDistance <= this.stopChaseDistance)
        {
            Util.Instance.SlowingDownWithAcceleration(this.rb, this.chaseSpeed, this.slowDownTime);
        }
        else
        {
            Util.Instance.MoveWithAcceleration(this.rb, this.moveDir, this.chaseSpeed, this.speedUpTime, this.slowDownTime);
            this.isChasingTarget = true;
        }
    }

    //============================================Jump============================================
    protected virtual void Jumping()
    {
        if (this.isWallDetected && this.isGround) this.Jump();
        if (this.isJumping && this.rb.velocity.y <= Mathf.Pow(0.1f, 3)) this.isJumping = false;
    }

    protected virtual void Jump()
    {
        Util.Instance.Jump(this.rb, this.jumpSpeed);
        this.isJumping = true;
    }

    //========================================Shoot Arrow=========================================
    protected virtual void UsingBow()
    {
        if (this.target == null)
        {
            this.isChargingBow = false;
            this.chargeBowCD.ResetStatus();
            return;
        }

        this.ChargingBow();
        this.RestoringBow();
    }

    protected virtual void RestoringBow()
    {
        if (this.isChargingBow) return;
        this.bowRestoreCD.CoolingDown();
        float currDistance = Vector2.Distance(this.target.position, transform.position);

        if (!this.bowRestoreCD.IsReady || this.target == null || currDistance > this.attackDistance) return;
        this.isChargingBow = true;
        this.bowRestoreCD.ResetStatus();
    }

    protected virtual void ShootArrow()
    {
        Vector2 spawnPos = this.shootPoint.position;
        float xDistance = this.target.position.x - transform.position.x;
        float zRot = xDistance > 0 ? 0 : 180;
        Quaternion spawnRot = Quaternion.Euler(0, 0, zRot);
        Debug.Log(xDistance, gameObject);
        Debug.Log(zRot, gameObject);
        Debug.Log(spawnRot, gameObject);

        Transform newArrow = BulletSpawner.Instance.SpawnByName(this.arrowName, spawnPos, spawnRot);
        newArrow.gameObject.SetActive(true);
    }

    protected virtual void ChargingBow()
    {
        if (!this.isChargingBow) return;
        this.chargeBowCD.CoolingDown();

        if (!this.chargeBowCD.IsReady) return;
        this.ShootArrow();
        this.isChargingBow = false;
        this.chargeBowCD.ResetStatus();
    }
}
