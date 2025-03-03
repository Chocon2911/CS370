using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMovement_2 : IMovement
{
    int GetMoveDir(PlayerMovement_2 component);
    bool GetIsJump(PlayerMovement_2 component);
    bool CanMove(PlayerMovement_2 component);
    bool CanJump(PlayerMovement_2 component);
}

public class PlayerMovement_2 : Movement
{
    //==========================================Variable==========================================
    [Header("===Player_2===")]
    [SerializeField] protected InterfaceReference<IPlayerMovement_2> user1;
    
    [Header("Move")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int moveDir;
    [SerializeField] protected float moveSpeedUpTime;
    [SerializeField] protected float moveSlowDownTime;

    [Header("Jump")]
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected bool isGround;
    [SerializeField] protected bool isJumping;

    [Header("Ground Check")]
    [SerializeField] protected string groundTag;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected CapsuleCollider2D grouncCol; 

    //==========================================Get Set===========================================
    public IPlayerMovement_2 User1 { set => user1.Value = value; }

    //===========================================Unity============================================
    public override void LoadComponents() 
    { 
        base.LoadComponents();
        this.LoadComponent(ref this.grouncCol, transform.Find("GroundCheck"), "LoadGrouncCol()");
    }

    public override void MyFixedUpdate()
    {
        base.MyFixedUpdate();
    }

    public override void MyUpdate()
    {
        base.MyUpdate();
        this.CheckingGround();
        this.Moving();
        this.Jumping();
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
        if (!this.user1.Value.CanMove(this)) return;
        float xVel = this.user1.Value.GetRb(this).velocity.x;
        this.moveDir = this.user1.Value.GetMoveDir(this);

        if ((this.moveDir > 0 && xVel > -Mathf.Pow(0.1f, 3))
            || (this.moveDir < 0 && xVel < Mathf.Pow(0.1f, 3))) this.Move();
        else this.StopMove();
    }

    protected virtual void Move()
    {
        Rigidbody2D rb = this.user1.Value.GetRb(this);
        float xVel = rb.velocity.x;
        float addVel = (Time.deltaTime / this.moveSpeedUpTime) * this.moveSpeed * this.moveDir;
        float leftVel = (this.moveDir * this.moveSpeed) - xVel;

        if (Mathf.Abs(addVel) > Mathf.Abs(leftVel)) addVel = leftVel;
        rb.velocity += new Vector2(addVel, 0);
    }

    public virtual void StopMove()
    {
        Rigidbody2D rb = this.user1.Value.GetRb(this);
        float deduceVel = (Time.deltaTime / this.moveSlowDownTime) * this.moveSpeed;
        float xVel = rb.velocity.x;

        if (xVel < 0) deduceVel *= -1;
        if (Mathf.Abs(deduceVel) > Mathf.Abs(xVel)) deduceVel = xVel;
        rb.velocity -= new Vector2(deduceVel, 0);
    }

    //============================================Jump============================================
    protected virtual void Jumping()
    {
        if (!this.user1.Value.CanJump(this)) return;
        float yVel = this.user1.Value.GetRb(this).velocity.y;

        if (this.isGround || this.isJumping) this.isJumping = this.user1.Value.GetIsJump(this);
        else this.isJumping = false;

        if (!this.isGround && !this.isJumping && yVel > 0) this.StopJump();
        else if (this.isGround && this.isJumping) this.Jump();
        else return;
    }

    protected virtual void Jump()
    {
        Rigidbody2D rb = this.user1.Value.GetRb(this);
        float yVel = this.jumpSpeed;
        float xVel = rb.velocity.x;

        rb.velocity = new Vector2(xVel, yVel);
        this.isGround = false;
    }

    protected virtual void StopJump()
    {
        Rigidbody2D rb = this.user1.Value.GetRb(this);
        float xVel = rb.velocity.x;
        rb.velocity = new Vector2(xVel, 0);
    }

    //========================================Ground Check========================================
    protected virtual void CheckingGround()
    {
        Vector2 size = this.grouncCol.size;
        Vector2 pos = this.grouncCol.transform.position;
        CapsuleDirection2D dir = this.grouncCol.direction;
        float angle = 0;


        Collider2D[] targetCols = Physics2D.OverlapCapsuleAll(
            pos, 
            size, 
            dir,
            angle, 
            this.groundLayer);

        foreach (Collider2D targetCol in targetCols)
        {
            if (targetCol.tag != this.groundTag) continue;
            this.isGround = true;
            return;
        }

        this.isGround = false;
    }
}
