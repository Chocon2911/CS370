using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream:Assets/Script/Obj/Entity/Monster/Aborigine.cs
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Aborigine : Entity
=======
public abstract class Monster : Entity, Damagable, EffectSplashable
>>>>>>> Stashed changes:Assets/Script/Obj/Entity/Monster/Monster.cs
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Monster===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
<<<<<<< Updated upstream:Assets/Script/Obj/Entity/Monster/Aborigine.cs
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
=======
    [SerializeField] protected CapsuleCollider2D bodyCol;
    [SerializeField] protected ParticleSystem damageEff;
>>>>>>> Stashed changes:Assets/Script/Obj/Entity/Monster/Monster.cs

    [Header("Target Out Of Range")]
    [SerializeField] protected Vector2 targetDetectingArea;

<<<<<<< Updated upstream:Assets/Script/Obj/Entity/Monster/Aborigine.cs
=======
    [Space(25)]

    [Header("Target Detection")]
    [SerializeField] protected Transform target;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected string targetTag = "Player";

    [Space(25)]

>>>>>>> Stashed changes:Assets/Script/Obj/Entity/Monster/Monster.cs
    [Header("Move Randomly")]
    [SerializeField] protected float slowSpeed;
    [SerializeField] protected List<Transform> endPoints = new List<Transform>();
    [SerializeField] protected int currEndPoint;
    [SerializeField] protected bool isMovingRandomly;

    [Header("Chase Target")]
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected bool isChasingTarget;

<<<<<<< Updated upstream:Assets/Script/Obj/Entity/Monster/Aborigine.cs
    [Header("Move")]
    [SerializeField] protected int moveDir;
    [SerializeField] protected float slowDownTime;
    [SerializeField] protected float speedUpTime;

    [Header("Jump")]
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected bool isJumping;

    
=======
    //==========================================Get Set===========================================
    // ===Move Randomly===
    public float SlowSpeed => this.slowSpeed;
    public bool IsMovingRandomly => this.isMovingRandomly;

    // ===Chase Target===
    public float ChaseSpeed => this.chaseSpeed;
    public bool IsChasingTarget => this.isChasingTarget;
>>>>>>> Stashed changes:Assets/Script/Obj/Entity/Monster/Monster.cs

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
<<<<<<< Updated upstream:Assets/Script/Obj/Entity/Monster/Aborigine.cs
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
=======
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.LoadComponent(ref this.damageEff, transform.Find("DamageEffect"), "LoadDamageEff()");
    }


>>>>>>> Stashed changes:Assets/Script/Obj/Entity/Monster/Monster.cs

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

<<<<<<< Updated upstream:Assets/Script/Obj/Entity/Monster/Aborigine.cs
    //=======================================Move Randomly========================================
    protected virtual void MoveRandomly()
    {
        if (this.currEndPoint + 1 == this.endPoints.Count && this.IsReachedEndPoint()) this.currEndPoint = 0;
        else if (this.IsReachedEndPoint()) this.currEndPoint++;
        this.moveDir = this.endPoints[this.currEndPoint].position.x > transform.position.x ? 1 : -1;
    }

=======
    //============================================Move============================================
>>>>>>> Stashed changes:Assets/Script/Obj/Entity/Monster/Monster.cs
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

<<<<<<< Updated upstream:Assets/Script/Obj/Entity/Monster/Aborigine.cs
    //=======================================Face Direction=======================================
    protected virtual void Facing()
    {
        if (this.moveDir == 0) return;
        Util.Instance.RotateFaceDir(this.moveDir, this.transform);
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
            Util.Instance.MoveWithAcceleration(this.rb, this.moveDir, this.walkSpeed, this.speedUpTime, this.slowDownTime);
            this.isWalking = true;
        }

        else
        {
            this.ChaseTarget();
            Util.Instance.MoveWithAcceleration(this.rb, this.moveDir, this.chaseSpeed, this.speedUpTime, this.slowDownTime);
            this.isChasingTarget = true;
        }
    }

    //============================================Jump============================================
    protected virtual void Jumping()
    {
        if (this.isWallDetected && this.isGround) this.Jump();

        if (this.isJumping)
        {
            if (this.rb.velocity.y <= Mathf.Pow(0.1f, 3)) this.isJumping = false;
        }
    }

    protected virtual void Jump()
    {
        Util.Instance.Jump(this.rb, this.jumpSpeed);
        this.isJumping = true;
=======
    //=======================================Damage Effect========================================
    protected virtual void PlayDamageEffect(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.damageEff.transform.rotation = Quaternion.Euler(0, 0, angle - 20f);
        this.damageEff.Play();
    }



    //============================================================================================
    //=========================================Interface==========================================
    //============================================================================================

    //=========================================Damagable==========================================
    void Damagable.TakeDamage(int damage)
    {
        this.health -= damage;
        this.damageEff.Play();

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
>>>>>>> Stashed changes:Assets/Script/Obj/Entity/Monster/Monster.cs
    }
}
