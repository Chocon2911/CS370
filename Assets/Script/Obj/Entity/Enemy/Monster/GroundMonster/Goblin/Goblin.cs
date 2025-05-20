using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UIElements;

public enum HitState
{
    NULL = 0,
    CHARGE = 1,
    ATTACK = 2,
    FINISH = 3,
}

public class Goblin : GroundMonster, Damagable
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Goblin===")]
    [Header("Component")]
    [SerializeField] protected GoblinAnimator animator;
    [SerializeField] protected GoblinSO so;

    [Space(25)]

    [Header("Stat")]
    [SerializeField] protected List<string> attackableTags;
    [SerializeField] protected LayerMask attackabelLayer;

    [Space(25)]

    [Header("Chase Target")]
    [SerializeField] protected float stopChaseDistance;
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected bool isChasingTarget;

    [Space(25)]

    [Header("Attacked")]
    [SerializeField] protected Cooldown attackedCD;
    [SerializeField] protected bool isAttackedPush;

    [Space(25)]

    [Header("Hit")]
    [SerializeField] protected CircleCollider2D hitCol;
    [SerializeField] protected int hitDamage;
    [SerializeField] protected float hitPushForce;
    [SerializeField] protected float hitRange;
    [SerializeField] protected Cooldown hitRestoreCD;
    [SerializeField] protected Cooldown hitChargeCD;
    [SerializeField] protected Cooldown hitAttackCD;
    [SerializeField] protected Cooldown hitFinishCD;
    [SerializeField] protected bool isHitting;
    [SerializeField] protected HitState currHitState;
    // Start is called before the first frame update

    //==========================================Get Set===========================================
    // Component
    public Rigidbody2D Rb => rb;

    // Attacked
    public bool IsAttackedPush => this.isAttackedPush;

    // ===Chase Target===
    public float ChaseSpeed => this.chaseSpeed;
    public bool IsChasingTarget => this.isChasingTarget;


    // Cut
    public Cooldown HitChargeCD => this.hitChargeCD;
    public Cooldown HitAttackCD => this.hitAttackCD;
    public Cooldown HitFinishCD => this.hitFinishCD;
    public bool IsHitting => this.isHitting;
    public HitState CurrHitState => this.currHitState;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform.Find("Model"), "LoadAnimator()");
        this.LoadComponent(ref this.hitCol, transform.Find("Hit"), "LoadCutCol()");
        this.DefaultGoblinStat();
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if (this.health <= 0) return;
        base.OnCollisionStay2D(collision);
    }

    protected override void Update()
    {
        base.Update();

        if (this.health > 0)
        {
            this.DetectingWall();
            this.CheckingIsGround();
            this.DetectingTarget();
            this.CheckingTargetOutOfRange();
            this.Facing();
            this.Moving();
            this.Hitting();
            //this.Jumping();
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

    protected virtual void DefaultGoblinStat()
    {
        if (this.so == null)
        {
            Debug.LogError("SO is null");
            return;
        }
        this.DefaultMonsterStat(this.so);
        this.DefaultGroundMonsterStat(this.so);

        // chase target
        this.stopChaseDistance = so.StopChaseDistance;
        this.chaseSpeed = so.ChaseSpeed;

        // attack
        this.hitCol.radius = this.so.HitRadius;
        this.hitDamage = this.so.HitDamage;
        this.hitPushForce = this.so.HitPushForce;
        this.hitRange = this.so.HitRange;
        this.hitRestoreCD = new Cooldown(this.so.HitRestoreCD, 0);
        this.hitAttackCD = new Cooldown(this.so.HitAttackCD, 0);
        this.hitChargeCD = new Cooldown(this.so.HitChargeCD, 0);
        this.hitFinishCD = new Cooldown(this.so.HitFinishCD, 0);
        this.attackabelLayer = this.so.AttackableLayer;
        this.attackableTags = new List<string>(this.so.AttackableTags);
    }

    //==========================================Attacked==========================================
    protected virtual void HandlingAttacked()
    {
        if (!this.isAttackedPush) return;
        this.attackedCD.CoolingDown();

        if (!this.attackedCD.IsReady) return;
        this.attackedCD.ResetStatus();
        this.isAttackedPush = false;
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
    protected virtual void Hitting()
    {
        this.RestoringHit();
        this.ChargingHit();
        this.HitAttacking();
        this.HitFinishing();

        if (!this.hitRestoreCD.IsReady || this.target == null) return;
        float distance = Vector2.Distance(this.transform.position, this.target.transform.position);

        if (distance > this.hitRange) return;
        this.isHitting = true;
        this.currHitState = HitState.CHARGE;
        this.hitRestoreCD.ResetStatus();
    }

    protected virtual void RestoringHit()
    {
        if (this.hitRestoreCD.IsReady || this.isHitting) return;
        this.hitRestoreCD.CoolingDown();
    }

    protected virtual void ChargingHit()
    {
        if (this.currHitState != HitState.CHARGE) return;
        this.hitChargeCD.CoolingDown();

        if (!this.hitChargeCD.IsReady) return;
        this.currHitState = HitState.ATTACK;
        this.hitChargeCD.ResetStatus();
        this.rb.velocity = Vector2.zero;
    }

    protected virtual void HitAttacking()
    {
        if (this.currHitState != HitState.ATTACK) return;
        this.hitAttackCD.CoolingDown();
        this.HitColliding();

        if (!this.hitAttackCD.IsReady) return;
        this.currHitState = HitState.FINISH;
        this.hitAttackCD.ResetStatus();
    }

    protected virtual void HitFinishing()
    {
        if (this.currHitState != HitState.FINISH) return;
        this.hitFinishCD.CoolingDown();

        if (!this.hitFinishCD.IsReady) return;
        this.currHitState = HitState.NULL;
        this.isHitting = false;
        this.hitFinishCD.ResetStatus();
    }

    protected virtual void HitColliding()
    {
        Vector2 pos = this.hitCol.transform.position;
        float rad = this.hitCol.radius * transform.localScale.x;

        Collider2D[] collisions = Physics2D.OverlapCircleAll(pos, rad, this.attackabelLayer);

        foreach (Collider2D collision in collisions)
        {
            if (this.attackableTags.Contains(collision.tag))
            {
                Damagable damagable = collision.GetComponent<Damagable>();

                if (damagable == null) return;
                damagable.TakeDamage(this.hitDamage);

                float xDir = collision.transform.position.x - this.transform.position.x;
                float yDir = collision.transform.position.y - this.transform.position.y;
                Vector2 dir = new Vector2(xDir, yDir).normalized;
                damagable.Push(dir * this.hitPushForce);
            }
        }
    }

    //=========================================Damagable==========================================
    void Damagable.Push(Vector2 force)
    {
        this.rb.velocity = force;
        float pushSpeed = force.x >= force.y ? force.x : force.y;
        float currSpeed = this.isMovingRandomly ? this.slowSpeed : this.chaseSpeed;
        float pushTime = pushSpeed / currSpeed * this.slowDownTime;
        this.attackedCD.TimeLimit = pushTime;
        this.isAttackedPush = true;
    }


}
