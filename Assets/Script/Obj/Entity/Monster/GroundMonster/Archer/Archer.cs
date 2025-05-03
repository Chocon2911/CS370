using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Archer : GroundMonster
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Archer===")]
    [Header("Component")]
    [SerializeField] protected ArcherAnimator animator;
    [SerializeField] protected ArcherSO so;

    [Space(25)]

    [Header("Bow Attack")]
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected string arrowName;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected Cooldown bowRestoreCD;
    [SerializeField] protected Cooldown chargeBowCD;
    [SerializeField] protected bool isChargingBow;

    //==========================================Get Set===========================================    
    // ===Bow Attack===
    public Cooldown ChargBowCD => this.chargeBowCD;
    public bool IsChargingBow => this.isChargingBow;



    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform.Find("Model"), "LoadModel()");
        this.LoadComponent(ref this.shootPoint, transform.Find("ShootPoint"), "LoadShootPoint()");
        this.DefaultArcherStat();
    }

    protected virtual void Update()
    {
        this.DetectingWall();
        this.CheckingIsGround();
        this.DetectingTarget();
        this.CheckingTargetOutOfRange();
        this.Facing();
        this.Moving();
        //this.Jumping();
        this.UsingBow();
        this.animator.HandlingAnimator();
    }



    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //===========================================Other============================================
    protected virtual void DefaultArcherStat()
    {
        if (this.so == null)
        {
            Debug.LogError("Archer SO is null", gameObject);
            return;
        }

        // bow attack
        this.attackDistance = this.so.AttackDistance;
        this.bowRestoreCD = new Cooldown(this.so.BowRestoreDelay, 0);
        this.chargeBowCD = new Cooldown(this.so.ChargeBowDelay, 0);
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
}
