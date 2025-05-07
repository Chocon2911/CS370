using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UIElements;

public enum CutState
{
    NULL = 0,
    CHARGE = 1,
    ATTACK = 2,
    FINISH = 3,
}

public class Undead : GroundMonster, Damagable
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Undead===")]
    [Header("Component")]
    [SerializeField] protected UndeadAnimator animator;

    [Space(25)]

    [Header("Stat")]
    [SerializeField] protected List<string> attackableTags;
    [SerializeField] protected LayerMask attackabelLayer;

    [Space(25)]

    [Header("Attacked")]
    [SerializeField] protected Cooldown attackedCD;
    [SerializeField] protected bool isAttackedPush;

    [Space(25)]

    [Header("Bite")]
    [SerializeField] protected CircleCollider2D cutCol;
    [SerializeField] protected int cutDamage;
    [SerializeField] protected float cutPushForce;
    [SerializeField] protected float cutRange;
    [SerializeField] protected Cooldown cutRestoreCD;
    [SerializeField] protected Cooldown cutChargeCD;
    [SerializeField] protected Cooldown cutAttackCD;
    [SerializeField] protected Cooldown cutFinishCD;
    [SerializeField] protected bool isCutting;
    [SerializeField] protected CutState currCutState;
    // Start is called before the first frame update

    //==========================================Get Set===========================================
    // Component
    public Rigidbody2D Rb => rb;
    
    // Attacked
    public bool IsAttackedPush => this.isAttackedPush;

    // Standing


    // Cut
    public Cooldown CutChargeCD => this.cutChargeCD;
    public Cooldown CutAttackCD => this.cutAttackCD;
    public Cooldown CutFinishCD => this.cutFinishCD;
     public bool IsCutting => this.isCutting;
    public CutState CurrCutState => this.currCutState;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform.Find("Model"), "LoadAnimator()");
        this.LoadComponent(ref this.cutCol, transform.Find("Cut"), "LoadCutCol()");
    }

    protected virtual void Update()
    {
        this.DetectingWall();
        this.CheckingIsGround();
        this.DetectingTarget();
        this.CheckingTargetOutOfRange();
        this.Facing();
        this.Moving();
        this.Cutting();
        //this.Jumping();
        this.animator.HandlingAnimator();

        // Huy temp
        if (this.health <= 0)
        {
            this.rb.velocity = new Vector2(0, this.rb.velocity.y);
        }
    }

    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================


    //==========================================Attacked==========================================
    protected virtual void HandlingAttacked()
    {
        if (!this.isAttackedPush) return;
        this.attackedCD.CoolingDown();

        if (!this.attackedCD.IsReady) return;
        this.attackedCD.ResetStatus();
        this.isAttackedPush = false;
    }

    //============================================Bite============================================
    protected virtual void Cutting()
    {
        this.RestoringCut();
        this.ChargingCut();
        this.CutAttacking();
        this.CutFinishing();

        if (!this.cutRestoreCD.IsReady || this.target == null) return;
        float distance = Vector2.Distance(this.transform.position, this.target.transform.position);

        if (distance > this.cutRange) return;
        this.isCutting = true;
        this.currCutState = CutState.CHARGE;
        this.cutRestoreCD.ResetStatus();
    }

    protected virtual void RestoringCut()
    {
        if (this.cutRestoreCD.IsReady || this.isCutting) return;
        this.cutRestoreCD.CoolingDown();
    }

    protected virtual void ChargingCut()
    {
        if (this.currCutState != CutState.CHARGE) return;
        this.cutChargeCD.CoolingDown();

        if (!this.cutChargeCD.IsReady) return;
        this.currCutState = CutState.ATTACK;
        this.cutChargeCD.ResetStatus();
        this.rb.velocity = Vector2.zero;
    }

    protected virtual void CutAttacking()
    {
        if (this.currCutState != CutState.ATTACK) return;
        this.cutAttackCD.CoolingDown();
        this.CutColliding();

        if (!this.cutAttackCD.IsReady) return;
        this.currCutState = CutState.FINISH;
        this.cutAttackCD.ResetStatus();
    }

    protected virtual void CutFinishing()
    {
        if (this.currCutState != CutState.FINISH) return;
        this.cutFinishCD.CoolingDown();

        if (!this.cutFinishCD.IsReady) return;
        this.currCutState = CutState.NULL;
        this.isCutting = false;
        this.cutFinishCD.ResetStatus();
    }

    protected virtual void CutColliding()
    {
        Vector2 pos = this.cutCol.transform.position;
        float rad = this.cutCol.radius;

        Collider2D[] collisions = Physics2D.OverlapCircleAll(pos, rad, this.attackabelLayer);

        foreach (Collider2D collision in collisions)
        {
            if (this.attackableTags.Contains(collision.tag))
            {
                Damagable damagable = collision.GetComponent<Damagable>();

                if (damagable == null) return;
                damagable.TakeDamage(this.cutDamage);

                float xDir = collision.transform.position.x - this.transform.position.x;
                float yDir = collision.transform.position.y - this.transform.position.y;
                Vector2 dir = new Vector2(xDir, yDir).normalized;
                damagable.Push(dir * this.cutPushForce);
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

