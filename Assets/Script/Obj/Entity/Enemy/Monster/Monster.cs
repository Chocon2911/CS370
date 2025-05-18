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
    [SerializeField] protected BoxCollider2D targetDetectingArea;

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



    //==========================================Get Set===========================================
    // ===Move Randomly===
    public float SlowSpeed => this.slowSpeed;
    public bool IsMovingRandomly => this.isMovingRandomly;



    //===========================================Unity============================================
    protected override void OnEnable()
    {
        base.OnEnable();
        gameObject.layer = LayerMask.NameToLayer("Monster");
    }

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.targetDetectingArea, transform.Find("OutRange"), "LoadTargetDetectingArea()");
    }



    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //===========================================Other============================================
    protected virtual void DefaultMonsterStat(MonsterSO so)
    {
        this.DefaultEnenmy(so);

        // target out of range
        this.targetDetectingArea.size = so.TargetDetectingArea;

        // target detection
        this.targetLayer = so.TargetLayer;
        this.targetTag = so.TargetTag;

        // move
        this.slowDownTime = so.SlowDownTime;
        this.speedUpTime = so.SpeedUpTime;

        // move randomly
        this.slowSpeed = so.SlowSpeed;
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

        Debug.Log(xDistance, gameObject);
        Debug.Log(this.targetDetectingArea.size.x, gameObject);

        if (xDistance > this.targetDetectingArea.size.x / 2 || yDistance > this.targetDetectingArea.size.y / 2)
        {
            this.target = null;
            this.DetectingTarget();
        }
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