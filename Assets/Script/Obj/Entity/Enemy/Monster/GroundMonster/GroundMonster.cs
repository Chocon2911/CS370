using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundMonster : Monster
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Ground Monster===")]
    [Header("Component")]
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

    [Space(25)]

    [Header("Jump")]
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected bool isJumping;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.groundCol, transform.Find("Ground"), "LoadGroundCol()");
    }

    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //===========================================Other============================================
    protected virtual void DefaultGroundMonsterStat(GroundMonsterSO so)
    {
        // ground check
        this.groundLayer = so.GroundLayer;
        this.groundTag = so.GroundTag;

        // wall detection
        this.wallDetectDistance = so.WallDetectDistance;
        this.wallLayer = so.WallLayer;
        this.wallTag = so.WallTag;

        // jump 
        this.jumpSpeed = so.JumpSpeed;
    }

    //======================================Ground Checking=======================================
    protected virtual void CheckingIsGround()
    {
        Util.Instance.CheckIsGround(this.groundCol, this.groundLayer, this.groundTag, ref this.prevIsGround, ref this.isGround, transform.localScale);
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

        if (this.target == null) this.MoveRandomly();
    }

    // ===Move Randomly===
    protected virtual void MoveRandomly()
    {
        if (this.endPoints.Count == 0 || this.endPoints[this.currEndPoint] == null) return;
        if (this.IsReachedEndPoint())
        {
            if (this.currEndPoint + 1 == this.endPoints.Count) this.currEndPoint = 0;
            else this.currEndPoint++;
        }

        this.moveDir = this.endPoints[this.currEndPoint].position.x > transform.position.x ? 1 : -1;
        this.isMovingRandomly = true;
        Util.Instance.MovingWithAccelerationInHorizontal(this.rb, this.moveDir, this.slowSpeed, this.speedUpTime, this.slowDownTime);
    }

    // ===End Points===
    protected override bool IsReachedEndPoint()
    {
        float currXPos = transform.position.x;
        float epXPos = this.endPoints[this.currEndPoint].position.x;

        float xDistance = Mathf.Abs(currXPos - epXPos);

        if (xDistance < 0.3f) return true;
        else return false;
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
}
