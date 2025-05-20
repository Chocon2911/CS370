using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class Fly : Monster
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Fly===")]
    [Header("Component")]
    [SerializeField] protected FlyAnimator animator;
    [SerializeField] protected TrailRenderer waveTrail;
    [SerializeField] protected FlySO so;

    [Space(25)]

    [Header("Run A Way")]
    [SerializeField] protected float runAwaySpeed;
    [SerializeField] protected float stopRunAwayDistance;

    [Space(25)]

    [Header("Target Detection")]
    [SerializeField] protected CircleCollider2D targetCol;

    [Space(25)]

    [Header("Move")]
    [SerializeField] protected Vector2 moveDir;

    [Space(25)]

    [Header("Fire")]
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected string bulletName;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected bool isFiring;
    [SerializeField] protected Cooldown fireRestoreCD;
    [SerializeField] protected Cooldown chargeFireCD;
    [SerializeField] protected bool isChargingFire;

    //==========================================Get Set===========================================
    // ===Fire===
    public Cooldown FireRestoreCD => this.fireRestoreCD;

    public Cooldown ChargeFireCD => this.chargeFireCD;

    public bool IsChargingFire => this.isChargingFire;

    public float RunAwaySpeed => this.runAwaySpeed;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.targetCol, transform.Find("Target"), "LoadTargetCol()");
        this.LoadComponent(ref this.animator, transform.Find("Model"), "LoadAnimator()");
        this.LoadComponent(ref this.waveTrail, transform.Find("Trail"), "LoadWaveTrail()");
        this.LoadComponent(ref this.shootPoint, transform.Find("ShootPoint"), "LoadShootPoint()");
        this.DefaultFlyStat();
    }

    protected override void Update()
    {
        if (this.health > 0)
        {
            base.Update();
            this.DetectingTarget();
            this.CheckingTargetOutOfRange();
            this.Facing();
            this.Moving();
            this.Firing();
        }
        else if (this.health <= 0)
        {
            this.rb.velocity = new Vector2(0, this.rb.velocity.y);
        }
        else if (this.health < 0)
        {
            this.Despawning();
        }
        this.animator.HandlingAnimator();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.waveTrail.emitting = false;
    }

    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //===========================================Other============================================
    protected virtual void DefaultFlyStat()
    {
        if (this.so == null)
        {
            Debug.LogError("Fly SO is null", gameObject);
            return;
        }

        this.DefaultMonsterStat(so);

        // Fire
        this.bulletName = this.so.BulletName;
        this.attackDistance = this.so.AttackDistance;
        this.fireRestoreCD = new Cooldown(this.so.FireRestoreDelay, 0);
        this.chargeFireCD = new Cooldown(this.so.ChargeFireDelay, 0);
    }

    //======================================Target Detection======================================
    protected override void DetectingTarget()
    {
        if (this.target != null) return;
        Vector2 pos = this.targetCol.transform.position;
        float rad = this.targetCol.radius * transform.localScale.x;

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
        this.isMovingRandomly = false;

        if (this.isFiring) return;

        if (this.target == null) this.MovingRandomly();
        else this.RunAwayTarget();
    }

    protected virtual void MovingRandomly()
    {
        if (this.endPoints.Count == 0 || this.endPoints[this.currEndPoint] == null) return;
        if (this.IsReachedEndPoint())
        {
            if (this.currEndPoint == this.endPoints.Count - 1) this.currEndPoint = 0;
            else this.currEndPoint++;
        }

        this.isMovingRandomly = true;
        this.Move(this.endPoints[this.currEndPoint], this.slowSpeed);
    }
    protected virtual void RunAwayTarget()
    {
        float currDistance = Mathf.Abs(transform.position.x - target.position.x);

        if (currDistance > this.stopRunAwayDistance)
        {
            Util.Instance.SlowingDownWithAccelerationInHorizontal(this.rb, this.runAwaySpeed, this.slowDownTime);
            Util.Instance.SlowingDownWithAccelerationInVertical(this.rb, this.runAwaySpeed, this.slowDownTime);
        }
        else
        {
            this.RunAway(this.target, this.runAwaySpeed);
        }
    }

    protected virtual void Move(Transform target, float speed)
    {
        float xDistance = target.position.x - transform.position.x;
        float yDistance = target.position.y - transform.position.y;

        this.moveDir = new Vector2(xDistance, yDistance).normalized;
        Util.Instance.FlyingWithAcceleration(this.rb, this.moveDir, speed, this.speedUpTime, this.slowDownTime);
    }

    protected virtual void RunAway(Transform target, float speed)
    {
        
        float xDistance = target.position.x - transform.position.x;
        int dir = xDistance < 0 ? 1 : -1;
        
        Util.Instance.MovingWithAccelerationInHorizontal(this.rb, dir, speed, this.speedUpTime, this.slowDownTime);
    }

    //============================================Gore============================================
    protected virtual void Firing()
    {
        if (this.target == null)
        {
            this.isChargingFire = false ;
            this.chargeFireCD.ResetStatus();
            return;
        }

        this.ChargingFire();
        this.RestoringFire();
    }

    protected virtual void RestoringFire()
    {
        if (this.isChargingFire) return;
        this.fireRestoreCD.CoolingDown();
        float currDistance = Vector2.Distance(this.target.position, transform.position);

        if (!this.fireRestoreCD.IsReady || this.target == null || currDistance > this.attackDistance) return;
        this.isChargingFire = true;
        this.fireRestoreCD.ResetStatus();
    }

    protected virtual void ChargingFire()
    {
        
        if (!this.isChargingFire) return;
        this.chargeFireCD.CoolingDown();

        
        if (!this.chargeFireCD.IsReady) return;
        this.FireAttacking();
        this.isChargingFire = false;
        this.chargeFireCD.ResetStatus();
    }

    protected virtual void FireAttacking()
    {
        Vector2 spawnPos = this.shootPoint.position;
        float xDistance = this.target.position.x - transform.position.x;
        float zRot = xDistance > 0 ? 0 : 180;
        Quaternion spawnRot = Quaternion.Euler(0, 0, zRot);
        Debug.Log(xDistance, gameObject);
        Debug.Log(zRot, gameObject);
        Debug.Log(spawnRot, gameObject);

        Transform newArrow = BulletSpawner.Instance.SpawnByName(this.bulletName, spawnPos, spawnRot);
        newArrow.gameObject.SetActive(true);
    }

}
