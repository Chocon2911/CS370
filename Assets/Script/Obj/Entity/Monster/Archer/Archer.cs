using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
<<<<<<<< HEAD:Assets/Script/Obj/Entity/Monster/Archer/Archer.cs
public class Archer : Monster
========
public class Monster : Entity, Damagable, EffectSplashable
>>>>>>>> main:Assets/Script/Obj/Entity/Monster/Monster.cs
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Archer===")]
    [Header("Component")]
<<<<<<<< HEAD:Assets/Script/Obj/Entity/Monster/Archer/Archer.cs
    [SerializeField] protected ArcherAnimator animator;
    [SerializeField] protected CapsuleCollider2D groundCol;
========
    [SerializeField] protected MonsterAnimator animator;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D col;
    [SerializeField] protected CapsuleCollider2D groundCol;
    [SerializeField] protected ParticleSystem damageEff;
    [SerializeField] protected Transform shootPoint;
>>>>>>>> main:Assets/Script/Obj/Entity/Monster/Monster.cs

    [Space(25)]

    [Header("Ground Check")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected string groundTag = "Terrain";
    [SerializeField] protected bool prevIsGround;
    [SerializeField] protected bool isGround;

    [Space(25)]

    [Header("Wall Detection")]
    [SerializeField] protected float wallDetectDistance;
    [SerializeField] protected bool isWallDetected;
    [SerializeField] protected LayerMask wallLayer;
    [SerializeField] protected string wallTag = "Terrain";

    [Space(25)]

    [Header("Target Detection")]
    [SerializeField] protected float targetDetectDistance;

    [Space(25)]
<<<<<<<< HEAD:Assets/Script/Obj/Entity/Monster/Archer/Archer.cs
========

    [Header("Target Out Of Range")]
    [SerializeField] protected Vector2 targetDetectingArea;

    [Space(25)]

    [Header("Move Randomly")]
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected List<Transform> endPoints = new List<Transform>();
    [SerializeField] protected int currEndPoint;
    [SerializeField] protected bool isWalking;

    [Space(25)]

    [Header("Chase Target")]
    [SerializeField] protected float stopChaseDistance;
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected bool isChasingTarget;
>>>>>>>> main:Assets/Script/Obj/Entity/Monster/Monster.cs

    [Space(25)]

    [Header("Move")]
    [SerializeField] protected int moveDir;
    [SerializeField] protected float slowDownTime;
    [SerializeField] protected float speedUpTime;

    [Space(25)]

    [Header("Jump")]
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected bool isJumping;

    [Space(25)]

    [Header("Bow Attack")]
<<<<<<<< HEAD:Assets/Script/Obj/Entity/Monster/Archer/Archer.cs
    [SerializeField] protected Transform shootPoint;
========
>>>>>>>> main:Assets/Script/Obj/Entity/Monster/Monster.cs
    [SerializeField] protected string arrowName;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected Cooldown bowRestoreCD;
    [SerializeField] protected Cooldown chargeBowCD;
    [SerializeField] protected bool isChargingBow;

    //==========================================Get Set===========================================    
<<<<<<<< HEAD:Assets/Script/Obj/Entity/Monster/Archer/Archer.cs
========
    // ===Move Randomly===
    public float WalkSpeed => this.walkSpeed;
    public bool IsWalking => this.isWalking;

    // ===Chase Target===
    public float ChaseSpeed => this.chaseSpeed;
    public bool IsChasingTarget => this.isChasingTarget;

>>>>>>>> main:Assets/Script/Obj/Entity/Monster/Monster.cs
    // ===Bow Attack===
    public Cooldown ChargBowCD => this.chargeBowCD;
    public bool IsChargingBow => this.isChargingBow;



    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform.Find("Model"), "LoadModel()");
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadCol()");
        this.LoadComponent(ref this.groundCol, transform.Find("Ground"), "LoadGroundCol()");
        this.LoadComponent(ref this.damageEff, transform.Find("DamageEffect"), "LoadDamageEff()");
        this.LoadComponent(ref this.shootPoint, transform.Find("ShootPoint"), "LoadShootPoint()");
    }

    protected virtual void Update()
    {
        this.DetectingWall();
        this.CheckingIsGround();
        this.DetectingTarget();
        this.CheckingTargetOutOfRange();
        this.Facing();
        this.Moving();
<<<<<<<< HEAD:Assets/Script/Obj/Entity/Monster/Archer/Archer.cs
        //this.Jumping();
========
        this.Jumping();
>>>>>>>> main:Assets/Script/Obj/Entity/Monster/Monster.cs
        this.UsingBow();
        this.animator.HandlingAnimator();
    }



    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //======================================Ground Checking=======================================
    protected virtual void CheckingIsGround()
    {
        Util.Instance.CheckIsGround(this.groundCol, this.groundLayer, this.groundTag, ref this.prevIsGround, ref this.isGround);
    }

    //=======================================Wall Detection=======================================
    protected virtual void DetectingWall()
    {
        float xDir = Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);
        float yDir = 0;
        Vector2 rayStart = transform.position + new Vector3(0, -0.5f, 0);
        Vector2 rayDir = new Vector2(xDir, yDir);

        Transform target = Util.Instance.ShootRaycast(this.wallDetectDistance, this.wallLayer, this.wallTag, rayStart, rayDir);
        this.isWallDetected = (target != null);
        Debug.DrawRay(rayStart, rayDir * wallDetectDistance, this.isWallDetected ? Color.green : Color.red);
    }

    //======================================Target Detection======================================
    protected override void DetectingTarget()
    {
        if (this.target != null) return;
        float xDir = Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);
        float yDir = 0;
        Vector2 rayStart = transform.position;
        Vector2 rayDir = new Vector2(xDir, yDir);

        this.target = Util.Instance.ShootRaycast(this.targetDetectDistance, this.targetLayer, this.targetTag, rayStart, rayDir);
        Debug.DrawRay(rayStart, rayDir * this.targetDetectDistance, this.target != null ? Color.green : Color.red);
    }

<<<<<<<< HEAD:Assets/Script/Obj/Entity/Monster/Archer/Archer.cs
    //=======================================Face Direction=======================================
    protected override void Facing()
========
    //====================================Target Out Of Range=====================================
    protected virtual void CheckingTargetOutOfRange()
    {
        if (this.target == null) return;
        float xDistance = Mathf.Abs(this.target.position.x - transform.position.x);
        float yDistance = Mathf.Abs(this.target.position.y - transform.position.y);

        if (xDistance > this.targetDetectingArea.x || yDistance > this.targetDetectingArea.y) this.target = null;
    }

    //=======================================Damage Effect========================================
    protected virtual void PlayDamageEffect(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.damageEff.transform.rotation = Quaternion.Euler(0, 0, angle - 20f);
        this.damageEff.Play();
    }

    //=======================================Face Direction=======================================
    protected virtual void Facing()
>>>>>>>> main:Assets/Script/Obj/Entity/Monster/Monster.cs
    {
        if (this.moveDir == 0) return;
        Util.Instance.RotateFaceDir(this.moveDir, this.transform);
    }

    //============================================Move============================================
<<<<<<<< HEAD:Assets/Script/Obj/Entity/Monster/Archer/Archer.cs
    protected override void Moving() 
    {
        this.isMovingRandomly = false;
========
    protected virtual void Moving() 
    {
        this.isWalking = false;
>>>>>>>> main:Assets/Script/Obj/Entity/Monster/Monster.cs
        this.isChasingTarget = false;

        if (this.target == null) this.MoveRandomly();
        else this.ChaseTarget();
    }

    // ===Move Randomly===
    protected virtual void MoveRandomly()
    {
        if (this.IsReachedEndPoint())
<<<<<<<< HEAD:Assets/Script/Obj/Entity/Monster/Archer/Archer.cs
        {
            if (this.currEndPoint + 1 == this.endPoints.Count) this.currEndPoint = 0;
            else this.currEndPoint++;
        }

        this.moveDir = this.endPoints[this.currEndPoint].position.x > transform.position.x ? 1 : -1;
        Util.Instance.MoveWithAcceleration(this.rb, this.moveDir, this.slowSpeed, this.speedUpTime, this.slowDownTime);
        this.isMovingRandomly = true;
    }

    // ===Chase Target===
    protected virtual void ChaseTarget()
    {
        float currDistance = Vector2.Distance(this.target.position, transform.position);
        this.moveDir = this.target.position.x > transform.position.x ? 1 : -1;

        if (currDistance <= this.stopChaseDistance)
        {
            Util.Instance.SlowingDownWithAcceleration(this.rb, this.chaseSpeed, this.slowDownTime);
        }
        else
        {
            Util.Instance.MoveWithAcceleration(this.rb, this.moveDir, this.chaseSpeed, this.speedUpTime, this.slowDownTime);
            this.isChasingTarget = true;
        }
========
        {
            if (this.currEndPoint + 1 == this.endPoints.Count) this.currEndPoint = 0;
            else this.currEndPoint++;
        }

        this.moveDir = this.endPoints[this.currEndPoint].position.x > transform.position.x ? 1 : -1;
        Util.Instance.MoveWithAcceleration(this.rb, this.moveDir, this.walkSpeed, this.speedUpTime, this.slowDownTime);
        this.isWalking = true;
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

    // ===Chase Target===
    protected virtual void ChaseTarget()
    {
        float currDistance = Vector2.Distance(this.target.position, transform.position);
        this.moveDir = this.target.position.x > transform.position.x ? 1 : -1;
        if (currDistance <= this.stopChaseDistance)
        {
            Util.Instance.StopMove(this.rb);
            return;
        }

        Util.Instance.MoveWithAcceleration(this.rb, this.moveDir, this.chaseSpeed, this.speedUpTime, this.slowDownTime);
        this.isChasingTarget = true;
>>>>>>>> main:Assets/Script/Obj/Entity/Monster/Monster.cs
    }

    //============================================Jump============================================
    protected virtual void Jumping()
    {
        if (this.isWallDetected && this.isGround) this.Jump();
        if (this.isJumping && this.rb.velocity.y <= Mathf.Pow(0.1f, 3)) this.isJumping = false;
    }

    protected virtual void Jump()
    {
        Util.Instance.Jump(this.rb, this.jumpSpeed);
        this.isJumping = true;
    }

    //========================================Shoot Arrow=========================================
    protected virtual void UsingBow()
    {
        if (this.target == null)
        {
            this.isChargingBow = false;
            this.chargeBowCD.ResetStatus();
            return;
        }

        this.ChargingBow();
        this.RestoringBow();
    }

    protected virtual void RestoringBow()
    {
        if (this.isChargingBow) return;
        this.bowRestoreCD.CoolingDown();
        float currDistance = Vector2.Distance(this.target.position, transform.position);

        if (!this.bowRestoreCD.IsReady || this.target == null || currDistance > this.attackDistance) return;
        this.isChargingBow = true;
        this.bowRestoreCD.ResetStatus();
    }

    protected virtual void ShootArrow()
    {
        Vector2 spawnPos = this.shootPoint.position;
        float xDistance = this.target.position.x - transform.position.x;
        float zRot = xDistance > 0 ? 0 : 180;
        Quaternion spawnRot = Quaternion.Euler(0, 0, zRot);
        Debug.Log(xDistance, gameObject);
        Debug.Log(zRot, gameObject);
        Debug.Log(spawnRot, gameObject);

        Transform newArrow = BulletSpawner.Instance.SpawnByName(this.arrowName, spawnPos, spawnRot);
        newArrow.gameObject.SetActive(true);
    }

    protected virtual void ChargingBow()
    {
        if (!this.isChargingBow) return;
        this.chargeBowCD.CoolingDown();

        if (!this.chargeBowCD.IsReady) return;
        this.ShootArrow();
        this.isChargingBow = false;
        this.chargeBowCD.ResetStatus();
    }
<<<<<<<< HEAD:Assets/Script/Obj/Entity/Monster/Archer/Archer.cs
========
 


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
    }
>>>>>>>> main:Assets/Script/Obj/Entity/Monster/Monster.cs
}
