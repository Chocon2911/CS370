using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public abstract class Monster : Enemy
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Monster===")]
    [Header("Target Out Of Range")]
    [SerializeField] protected Vector2 targetDetectingArea;

    [Space(25)]

    [Header("Target Detection")]
    [SerializeField] protected Transform target;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected string targetTag = "Player";

    [Space(25)]

    [Header("Move")]
    [SerializeField] protected float slowDownTime;
    [SerializeField] protected float speedUpTime;

    [Space(25)]

    [Header("Move Randomly")]
    [SerializeField] protected float slowSpeed;
    [SerializeField] protected List<Transform> endPoints = new List<Transform>();
    [SerializeField] protected int currEndPoint;
    [SerializeField] protected bool isMovingRandomly;

    [Space(25)]

    [Header("Chase Target")]
    [SerializeField] protected float stopChaseDistance;
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected bool isChasingTarget;



    //==========================================Get Set===========================================
    // ===Move Randomly===
    public float SlowSpeed => this.slowSpeed;
    public bool IsMovingRandomly => this.isMovingRandomly;

    // ===Chase Target===
    public float ChaseSpeed => this.chaseSpeed;
    public bool IsChasingTarget => this.isChasingTarget;



    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //===========================================Other============================================
    protected virtual void DefaultMonsterStat(MonsterSO so)
    {
        // target out of range
        this.targetDetectingArea = so.TargetDetectingArea;

        // target detection
        this.targetLayer = so.TargetLayer;
        this.targetTag = so.TargetTag;

        // move
        this.slowDownTime = so.SlowDownTime;
        this.speedUpTime = so.SpeedUpTime;

        // move randomly
        this.slowSpeed = so.SlowSpeed;

        // chase target
        this.stopChaseDistance = so.StopChaseDistance;
        this.chaseSpeed = so.ChaseSpeed;
    }

    //==========================================Abstract==========================================
    protected abstract void Moving();
    protected abstract void DetectingTarget();
    protected abstract void Facing();

    //====================================Target Out Of Range=====================================
    protected virtual void CheckingTargetOutOfRange()
    {
        if (this.target == null) return;
        float xDistance = Mathf.Abs(this.target.position.x - transform.position.x);
        float yDistance = Mathf.Abs(this.target.position.y - transform.position.y);

        if (xDistance > this.targetDetectingArea.x || yDistance > this.targetDetectingArea.y) this.target = null;
    }

    //============================================Move============================================
    protected virtual bool IsReachedEndPoint()
    {
        float currXPos = transform.position.x;
        float currYPos = transform.position.y;
        float epXPos = this.endPoints[this.currEndPoint].position.x;
        float epYPos = this.endPoints[this.currEndPoint].position.y;

        float xDistance = Mathf.Abs(currXPos - epXPos);
        float yDistance = Mathf.Abs(currYPos - epYPos);

        if (xDistance < 0.3f && yDistance < 0.3f) return true; 
        else return false;
    }
}