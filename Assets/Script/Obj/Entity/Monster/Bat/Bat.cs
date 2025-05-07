using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class Bat : Monster
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Bat===")]
    [SerializeField] protected BatAnimator animator;
    [SerializeField] protected TrailRenderer goreTrail;
    [SerializeField] protected BatSO so;

    [Space(25)]

    [Header("Target Detection")]
    [SerializeField] protected CircleCollider2D targetCol;

    [Space(25)]

    [Header("Move")]
    [SerializeField] protected Vector2 moveDir;

    [Space(25)]

    [Header("Gore")]
    [SerializeField] protected float goreSpeed;
    [SerializeField] protected float goreDistance;
    [SerializeField] protected Vector2 goreTargetpos;
    [SerializeField] protected Cooldown goreChargeCD;
    [SerializeField] protected Cooldown goreRestoreCD;
    [SerializeField] protected Cooldown goreAttackCD;
    [SerializeField] protected bool isGoring;
    [SerializeField] protected bool isChargingGore;
    [SerializeField] protected bool isGoreAttacking;

    //==========================================Get Set===========================================
    // ===Gore===
    public Cooldown GoreChargeCD => this.goreChargeCD;
    public bool IsGoring => this.isGoring;
    public bool IsChargingGore => this.isChargingGore;
    public bool IsGoreAttacking => this.isGoreAttacking; 

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.targetCol, transform.Find("Target"), "LoadTargetCol()");
        this.LoadComponent(ref this.animator, transform.Find("Model"), "LoadAnimator()");
        this.LoadComponent(ref this.goreTrail, transform.Find("Trail"), "LoadGoreTrail()");
    }

    protected virtual void Update()
    {
        this.DetectingTarget();
        this.CheckingTargetOutOfRange();
        this.Facing();
        this.Moving();
        this.Goring();
        this.animator.HandlingAnimator();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.goreTrail.emitting = false;
    }

    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //===========================================Other============================================
    protected virtual void DefaultBatStat()
    {
        if (this.so == null)
        {
            Debug.LogError("Bat SO is null", gameObject);
            return;
        }

        // Target Detection
        this.targetCol.radius = this.so.DetectionRad;

        // Gore
        this.goreSpeed = this.so.GoreSpeed;
        this.goreDistance = this.so.GoreDistance;
        this.goreRestoreCD = new Cooldown(this.so.GoreRestoreDelay, 0);
        this.goreChargeCD = new Cooldown(this.so.GoreChargeDelay, 0);
        this.goreAttackCD = new Cooldown(this.so.GoreAttackDelay, 0);
    }

    //======================================Target Detection======================================
    protected override void DetectingTarget()
    {
        if (this.target != null) return;
        Vector2 pos = this.targetCol.transform.position;
        float rad = this.targetCol.radius;

        Collider2D[] targets = Physics2D.OverlapCircleAll(pos, rad, this.targetLayer);
        
        foreach (Collider2D target in targets)
        {
            if (target.tag != this.targetTag) continue;
            this.target = target.transform;
            return;
        }
    }

    //=======================================Face Direction=======================================
    protected override void Facing()
    {
        if (this.moveDir.x == 0) return;
        Util.Instance.RotateFaceDir(this.moveDir.x > 0 ? 1 : -1, this.transform);
    }

    //============================================Move============================================
    protected override void Moving()
    {
        this.isChasingTarget = false;
        this.isMovingRandomly = false;

        if (this.isGoring) return;
    
        if (this.target == null) this.MovingRandomly();
        else this.ChasingTarget();
    }

    protected virtual void MovingRandomly()
    {
        this.isMovingRandomly = true;
        this.Move(this.endPoints[this.currEndPoint], this.slowSpeed);

        if (this.IsReachedEndPoint())
        {
            if (this.currEndPoint == this.endPoints.Count - 1) this.currEndPoint = 0;
            else this.currEndPoint++;
        }
    }
    
    protected virtual void ChasingTarget()
    {
        float currDistance = Vector2.Distance(this.transform.position, this.target.position);

        if (currDistance <= this.stopChaseDistance) return;
        this.isChasingTarget = true;
        this.Move(this.target, this.chaseSpeed);
    }

    protected virtual void Move(Transform target, float speed) 
    {
        float xDistance = target.position.x - transform.position.x;
        float yDistance = target.position.y - transform.position.y;

        this.moveDir = new Vector2(xDistance, yDistance).normalized;
        Util.Instance.FlyingWithAcceleration(this.rb, this.moveDir, speed, this.speedUpTime, this.slowDownTime);
    }

    //============================================Gore============================================
    protected virtual void Goring()
    {
        this.RestoringGore();
        this.ChargingGore();
        this.GoreAttacking();

        if (this.target == null || !this.goreRestoreCD.IsReady) return;
        this.goreRestoreCD.ResetStatus();
        this.isChargingGore = true;
        this.isGoring = true;
        this.goreTargetpos = this.target.position;
        this.rb.velocity = Vector2.zero;
    }

    protected virtual void RestoringGore()
    {
        if (this.goreRestoreCD.IsReady || this.isGoring) return;
        this.goreRestoreCD.CoolingDown();
    }

    protected virtual void ChargingGore()
    {
        if (!this.isChargingGore) return;
        this.goreChargeCD.CoolingDown();

        if (!this.goreChargeCD.IsReady) return;
        this.goreChargeCD.ResetStatus();
        this.isChargingGore = false;
        this.isGoreAttacking = true;
        this.moveDir = (this.goreTargetpos - (Vector2)this.transform.position).normalized;
        this.rb.velocity = this.moveDir * this.goreSpeed;
        this.goreTrail.emitting = true;
    }

    protected virtual void GoreAttacking()
    {
        if (!this.isGoreAttacking) return;
        this.goreAttackCD.CoolingDown();

        if (!this.goreAttackCD.IsReady) return;
        this.goreAttackCD.ResetStatus();
        this.isGoring = false;
        this.isGoreAttacking = false;
        this.goreTrail.emitting = false;
    }
}
