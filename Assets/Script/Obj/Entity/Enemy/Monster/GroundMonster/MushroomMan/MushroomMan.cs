using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMan : GroundMonster
{
    //==========================================Variable==========================================
    [Header("===Mushroom Man===")]
    [Header("Component")]
    [SerializeField] protected MushroomManAnimator animator;
    [SerializeField] protected MushroomManSO so;

    [Header("Stat")]
    [SerializeField] protected List<string> attackableTags;
    [SerializeField] protected LayerMask attackabelLayer;

    [Space(25)]

    [Header("Chase Target")]
    [SerializeField] protected float stopChaseDistance;
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected bool isChasingTarget;

    [Space(25)]

    [Header("Bite")]
    [SerializeField] protected CircleCollider2D biteCol;
    [SerializeField] protected int biteDamage;
    [SerializeField] protected float bitePushForce;
    [SerializeField] protected float biteRange;
    [SerializeField] protected Cooldown biteRestoreCD;
    [SerializeField] protected Cooldown biteChargeCD;
    [SerializeField] protected Cooldown biteAttackCD;
    [SerializeField] protected bool isBiting;
    [SerializeField] protected bool isChargingBite;
    [SerializeField] protected bool isBiteAttacking;

    //==========================================Get Set===========================================
    // ===Chase Target===
    public float ChaseSpeed => this.chaseSpeed;
    public bool IsChasingTarget => this.isChasingTarget;

    //===Bite===
    public Cooldown BiteChargeCD => this.biteChargeCD;
    public Cooldown BiteAttackCD => this.biteAttackCD;
    public bool IsBiting => this.isBiting;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform.Find("Model"), "LoadAnimator()");
        this.LoadComponent(ref this.biteCol, transform.Find("Bite"), "LoadBiteCol()");
        this.DefaultStat();
    }

    protected override void Update()
    {
        if (this.health > 0)
        {
            base.Update();
            this.DetectingWall();
            this.CheckingIsGround();
            this.DetectingTarget();
            this.CheckingTargetOutOfRange();
            this.Facing();
            this.Moving();
            this.Biting();
            //this.Jumping();
        }
        else if (this.health <= 0)
        {
            this.rb.velocity = new Vector2(0, this.rb.velocity.y);
        }
        this.animator.HandlingAnimator();
    }



    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //===========================================Other============================================
    protected override void Moving()
    {
        this.isChasingTarget = false;
        base.Moving();

        if (this.target != null) this.ChaseTarget();
    }

    protected virtual void DefaultStat()
    {
        if (this.so == null)
        {
            Debug.LogError("SO is null", gameObject);
            return;
        }

        this.DefaultMonsterStat(this.so);
        this.DefaultGroundMonsterStat(this.so);

        // chase target
        this.stopChaseDistance = this.so.StopChaseDistance;
        this.chaseSpeed = this.so.ChaseSpeed;

        // bite
        this.biteDamage = this.so.BiteDamage;
        this.bitePushForce = this.so.BitePushForce;
        this.biteRange = this.so.BiteRange;
        this.biteRestoreCD = new Cooldown(this.so.BiteRestoreDelay, 0);
        this.biteChargeCD = new Cooldown(this.so.BiteChargeDelay, 0);
        this.biteAttackCD = new Cooldown(this.so.BiteAttackDelay, 0);
    }

    //============================================Move============================================
    protected virtual void ChaseTarget()
    {
        float currDistance = Vector2.Distance(this.target.position, transform.position);
        this.moveDir = this.target.position.x > transform.position.x ? 1 : -1;

        if (currDistance <= this.stopChaseDistance)
        {
            Util.Instance.SlowingDownWithAccelerationInHorizontal(this.rb, this.chaseSpeed, this.slowDownTime);
        }
        else
        {
            Util.Instance.MovingWithAccelerationInHorizontal(this.rb, this.moveDir, this.chaseSpeed, this.speedUpTime, this.slowDownTime);
            this.isChasingTarget = true;
        }
    }

    //============================================Bite============================================
    protected virtual void Biting()
    {
        this.RestoringBite();
        this.ChargingBite();
        this.BiteAttacking();

        if (!this.biteRestoreCD.IsReady || this.target == null) return;
        float distance = Vector2.Distance(this.transform.position, this.target.transform.position);
        
        if (distance > this.biteRange) return;
        this.isBiting = true;
        this.isChargingBite = true;
        this.biteRestoreCD.ResetStatus();
    }

    protected virtual void RestoringBite()
    {
        if (this.biteRestoreCD.IsReady || this.isBiting) return;
        this.biteRestoreCD.CoolingDown();
    }

    protected virtual void ChargingBite()
    {
        if (!this.isChargingBite) return;
        this.biteChargeCD.CoolingDown();

        if (!this.biteChargeCD.IsReady) return;
        this.isChargingBite = false;
        this.isBiteAttacking = true;
        this.biteChargeCD.ResetStatus();
        this.rb.velocity = Vector2.zero;
    }

    protected virtual void BiteAttacking()
    {
        if (!this.isBiteAttacking) return;
        this.biteAttackCD.CoolingDown();
        this.BiteColliding();

        if (!this.biteAttackCD.IsReady) return;
        this.isBiting = false;
        this.isBiteAttacking = false;
        this.biteAttackCD.ResetStatus();
    }

    protected virtual void BiteColliding()
    {
        Vector2 pos = this.biteCol.transform.position;
        float rad = this.biteCol.radius;

        Collider2D[] collisions = Physics2D.OverlapCircleAll(pos, rad, this.attackabelLayer);

        foreach (Collider2D collision in collisions)
        {
            if (this.attackableTags.Contains(collision.tag))
            {
                Damagable damagable = collision.GetComponent<Damagable>();

                if (damagable == null) return;
                damagable.TakeDamage(this.biteDamage);

                float xDir = collision.transform.position.x - this.transform.position.x;
                float yDir = collision.transform.position.y - this.transform.position.y;
                Vector2 dir = new Vector2(xDir, yDir).normalized;
                damagable.Push(dir * this.bitePushForce);
            }
        }
    }
}
