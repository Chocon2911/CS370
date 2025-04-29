using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public abstract class Monster : Entity, Damagable, EffectSplashable
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Monster===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D bodyCol;
    [SerializeField] protected ParticleSystem damageEff;


    [Header("Target Out Of Range")]
    [SerializeField] protected Vector2 targetDetectingArea;

    [Space(25)]

    [Header("Target Detection")]
    [SerializeField] protected Transform target;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected string targetTag = "Player";

    [Space(25)]

    [Header("Move Randomly")]
    [SerializeField] protected float slowSpeed;
    [SerializeField] protected List<Transform> endPoints = new List<Transform>();
    [SerializeField] protected int currEndPoint;
    [SerializeField] protected bool isMovingRandomly;

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

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.LoadComponent(ref this.damageEff, transform.Find("DamageEffect"), "LoadDamageEff()");
    }



    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

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

    //=======================================Damage Effect========================================
    protected virtual void PlayDamageEffect(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.damageEff.transform.rotation = Quaternion.Euler(0, 0, angle - 2f);
        this.damageEff.Play();
    }



    //============================================================================================
    //=========================================Interface==========================================
    //============================================================================================

    //=========================================Damagable==========================================
    void Damagable.TakeDamage(int damage)
    {
        this.health -= damage;

        if (this.health <= 0)
        {
            this.health = 0;
            Debug.Log("Dead", gameObject);
        }
    }

    void Damagable.Push(Vector2 force)
    {
        this.rb.velocity = force;
    }

    //=====================================Effect Splashable======================================
    void EffectSplashable.Splash(Vector2 pos)
    {
        Vector2 dir = ((Vector2)transform.position - pos).normalized;
        this.PlayDamageEffect(dir);
    }
}