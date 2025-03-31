using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Aborigine : Entity
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Monster===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D col;

    [Header("Ground Check")]
    [SerializeField] protected CapsuleCollider2D groundCol;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected string groundTag = "Terrain";
    [SerializeField] protected bool prevIsGround;
    [SerializeField] protected bool isGround;

    [Header("Wall Detection")]
    [SerializeField] protected float wallDetectDistance;
    [SerializeField] protected bool isWallDetected;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] protected string wallTag = "Terrain";

    [Header("Target Detection")]
    [SerializeField] protected Transform target;
    [SerializeField] protected float targetDetectDistance;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected string targetTag = "Player";

    [Header("Target Out Of Range")]
    [SerializeField] protected Vector2 targetDetectingArea;

    [Header("Move Randomly")]
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected List<Transform> endPoints = new List<Transform>();
    [SerializeField] protected int currEndPoint;
    [SerializeField] protected bool isWalking;

    [Header("Chase Target")]
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected bool isChasingTarget;

    [Header("Move")]
    [SerializeField] protected int moveDir;
    [SerializeField] protected float slowDownTime;
    [SerializeField] protected float speedUpTime;

    [Header("Jump")]
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected bool isJumping;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.col, transform, "LoadCol()");
        this.LoadComponent(ref this.groundCol, transform.Find("Ground"), "LoadGroundCol()");
    }

    protected virtual void Update()
    {
        this.DetectingWall();
        this.CheckingIsGround();
        this.DetectingTarget();
        this.CheckingTargetOutOfRange();
        this.Facing();
        this.Moving();
        this.Jumping();
    }

    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //======================================Ground Checking=======================================
    protected virtual void CheckingIsGround()
    {
        UtilManager.Instance.CheckIsGround(this.groundCol, this.groundLayer, this.groundTag, ref this.prevIsGround, ref this.isGround);
    }

    //=======================================Wall Detection=======================================
    protected virtual void DetectingWall()
    {
        float xDir = Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);
        float yDir = 0;
        Vector2 rayStart = transform.position;
        Vector2 rayDir = new Vector2(xDir, yDir);

        Transform target = UtilManager.Instance.ShootRaycast(this.wallDetectDistance, this.wallLayer, this.wallTag, rayStart, rayDir);
        this.isWallDetected = (target != null);
        Debug.DrawRay(rayStart, rayDir * wallDetectDistance, this.isWallDetected ? Color.red : Color.green);
    }

    //======================================Target Detection======================================
    protected virtual void DetectingTarget()
    {
        if (this.target != null) return;
        float xDir = Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);
        float yDir = 0;
        Vector2 rayStart = transform.position;
        Vector2 rayDir = new Vector2(xDir, yDir);

        this.target = UtilManager.Instance.ShootRaycast(this.targetDetectDistance, this.targetLayer, this.targetTag, rayStart, rayDir);
        Debug.DrawRay(rayStart, rayDir * this.targetDetectDistance, this.target != null ? Color.green : Color.red);
    }

    //====================================Target Out Of Range=====================================
    protected virtual void CheckingTargetOutOfRange()
    {
        if (this.target == null) return;
        float xDistance = Mathf.Abs(this.target.position.x - transform.position.x);
        float yDistance = Mathf.Abs(this.target.position.y - transform.position.y);

        if (xDistance > this.targetDetectingArea.x || yDistance > this.targetDetectingArea.y) this.target = null;
    }

    //=======================================Move Randomly========================================
    protected virtual void MoveRandomly()
    {
        if (this.currEndPoint + 1 == this.endPoints.Count && this.IsReachedEndPoint()) this.currEndPoint = 0;
        else if (this.IsReachedEndPoint()) this.currEndPoint++;
        this.moveDir = this.endPoints[this.currEndPoint].position.x > transform.position.x ? 1 : -1;
    }

    protected virtual bool IsReachedEndPoint()
    {
        float currXPos = transform.position.x;
        float currYPos = transform.position.y;
        float epXPos = this.endPoints[this.currEndPoint].position.x;
        float epYPos = this.endPoints[this.currEndPoint].position.y;

        float xDistance = Mathf.Abs(currXPos - epXPos);
        float yDistance = Mathf.Abs(currYPos - epYPos);

        if (xDistance < 0.1f && yDistance < 0.1f) return true;
        else return false;
    }

    //=======================================Face Direction=======================================
    protected virtual void Facing()
    {
        if (this.moveDir == 0) return;
        UtilManager.Instance.RotateFaceDir(this.moveDir, this.transform);
    }

    //========================================Chase Target========================================
    protected virtual void ChaseTarget()
    {
        this.moveDir = this.target.position.x > transform.position.x ? 1 : -1;
    }

    //============================================Move============================================
    protected virtual void Moving() 
    {
        this.isWalking = false;
        this.isChasingTarget = false;

        if (this.target == null)
        {
            this.MoveRandomly();
            MovementManager.Instance.MoveWithAcceleration(this.rb, this.moveDir, this.walkSpeed, this.speedUpTime, this.slowDownTime);
            this.isWalking = true;
        }

        else
        {
            this.ChaseTarget();
            MovementManager.Instance.MoveWithAcceleration(this.rb, this.moveDir, this.chaseSpeed, this.speedUpTime, this.slowDownTime);
            this.isChasingTarget = true;
        }
    }

    //============================================Jump============================================
    protected virtual void Jumping()
    {
        if (this.isWallDetected) this.Jump();
    }

    protected virtual void Jump()
    {
        MovementManager.Instance.Jump(this.rb, this.jumpSpeed);
        this.isJumping = true;
    }
}
