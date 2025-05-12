using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("Slime")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D bodyCol;

    [Header("Stat")]
    [SerializeField] protected Transform target;

    [Header("Check Ground")]
    [SerializeField] protected CapsuleCollider2D groundCol;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected string groundTag;
    [SerializeField] protected bool isGround;

    [Header("Move")]
    [SerializeField] protected int moveDir;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float slowDownTime;
    [SerializeField] protected float speedUpTime;

    [Header("Jump")]
    [SerializeField] protected Cooldown jumpCD;

    [Header("Low Jump")]
    [SerializeField] protected float lowJumpSpeed;
    [SerializeField] protected bool isLowJumping;

    [Header("High Jump")]
    [SerializeField] protected float highJumpSpeed;
    [SerializeField] protected bool isHighJumping;

    [Header("First Attack")]
    // Landing Attack
    [SerializeField] protected float landSpeed;

    // Dash Attack
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected Cooldown dashCD;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
    }

    protected virtual void Update()
    {
        this.CheckingGround();
        this.LowJumping();
    }

    //========================================Check Ground========================================
    protected virtual void CheckingGround()
    {
        Vector2 point = this.groundCol.transform.position;
        Vector2 size = this.groundCol.size;
        CapsuleDirection2D direction = this.groundCol.direction;

        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(point, size, direction, 0, this.groundLayer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == this.groundTag)
            {
                this.isGround = true;
                return;
            }
        }
        this.isGround = false;
    }

    //==========================================Support===========================================
    protected virtual void Move()
    {
        Util.Instance.MovingWithAccelerationInHorizontal(this.rb, this.moveDir, this.moveSpeed, this.speedUpTime, this.slowDownTime);
    }

    protected virtual void StopMove()
    {
        Util.Instance.SlowingDownWithAccelerationInHorizontal(this.rb, this.moveSpeed, this.slowDownTime);
    }

    protected virtual void Jump(float jumpSpeed)
    {
        Util.Instance.Jump(this.rb, jumpSpeed);
    }

    //============================================Jump============================================
    protected virtual void Jumping()
    {

    }
    
    //===Low Jump===
    protected virtual void LowJumping()
    {
        if (!this.isLowJumping) return;
        this.Move();
    }

    //===High Jump===
    protected virtual void HighJumping()
    {

    }
}
