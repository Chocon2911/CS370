using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMovement_2 : IMovement
{
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
    [SerializeField] protected Jump jump;
    [SerializeField] protected bool isJumping;

    [Header("Component")]
    [SerializeField] protected GroundCheck groundCheck;

    //==========================================Get Set===========================================
    public IPlayerMovement_2 User1 { set => user1.Value = value; }
    public bool IsJumping => this.isJumping;

    //===========================================Unity============================================
    public override void LoadComponents() 
    { 
        base.LoadComponents();
        this.LoadComponent(ref this.groundCheck, transform.Find("GroundCheck"), "LoadGroundCheck()");
        this.LoadComponent(ref this.jump, transform.Find("Jump"), "LoadJump()");
    }

    public override void MyFixedUpdate()
    {
        base.MyFixedUpdate();
    }

    public override void MyUpdate()
    {
        base.MyUpdate();
        this.groundCheck.MyUpdate();
        this.Moving();
        this.Jumping();
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
        if (!this.user1.Value.CanMove(this)) return;
        float xVel = this.user1.Value.GetRb(this).velocity.x;
        this.moveDir = (int)InputManager.Instance.MoveDir.x;

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

        if (this.groundCheck.IsGround || this.isJumping) this.isJumping = InputManager.Instance.SpaceState != 0;
        else this.isJumping = false;
        Rigidbody2D rb = this.user1.Value.GetRb(this);

        if (!this.groundCheck.IsGround && !this.isJumping && yVel > 0) this.jump.StopJump(rb);
        else if (this.groundCheck.IsGround && this.isJumping) this.jump.DoJump(rb);
        else return;
    }
}
