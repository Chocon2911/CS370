using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public interface IPlayerMovement : IMovement
{
    bool CanMove(PlayerMovement component);
    bool CanStopMove(PlayerMovement component);
    bool CanJump(PlayerMovement component);
}

public class PlayerMovement : Movement
{
    //==========================================Variable==========================================
    [Header("===Player===")]
    [SerializeField] protected InterfaceReference<IPlayerMovement> user1;
    [Header("Move")]
    [SerializeField] protected int moveDir;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveSpeedUpTime;
    [SerializeField] protected float moveSlowDownTime;

    [Header("Jump")]
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected float jumpSlowDownTime;
    [SerializeField] protected Cooldown jumpCD;
    [SerializeField] protected bool isGround;
    [SerializeField] protected bool isJumping; 

    //==========================================Get Set===========================================
    public IPlayerMovement User1 { set => user1.Value = value; }

    //===========================================Unity============================================
    public override void MyUpdate()
    {
        base.MyUpdate();
        if (InputManager.Instance.ShiftState != 0) this.isGround = true;

        // Move
        this.StoppingMove();
        this.Moving();

        // Jump
        this.CheckingIsJumping();
        this.Jumping();
        this.StoppingJump();
        this.RechargingJump();
        this.FinishingJump();
    }



    //============================================================================================
    //============================================Move============================================
    //============================================================================================

    //============================================Move============================================
    protected virtual void Moving()
    {
        if (!this.user1.Value.CanMove(this)) return;
        this.Move();
    }

    protected virtual void Move()
    {
        this.moveDir = (int)InputManager.Instance.MoveDir.x;
        
        if (this.moveDir == 0) return;
        Vector2 force = new Vector2(this.GetXForce(), 0);
        this.user1.Value.GetRb(this).AddForce(force, ForceMode2D.Impulse);
    }

    protected virtual float GetXForce()
    {
        Rigidbody2D rb = this.user1.Value.GetRb(this);
        float xVel = rb.velocity.x;

        if (this.moveDir > 0)
        {
            if (xVel >= this.moveSpeed) return 0;
            if (xVel < -Mathf.Pow(0.1f, 3)) return 0;
        }
        else if (this.moveDir < 0)
        {
            if (xVel <= -this.moveSpeed) return 0;
            if (xVel > Mathf.Pow(0.1f, 3)) return 0;
        }

        float xForce = this.moveDir * this.moveSpeed / moveSpeedUpTime * Time.deltaTime;
        float xVelLeft = this.moveSpeed - Mathf.Abs(xVel);

        if (this.moveDir > 0 && xForce > xVelLeft) xForce = xVelLeft;
        else if (this.moveDir < 0 && -xForce > xVelLeft) xForce = -xVelLeft;
        return xForce;
    }

    //=========================================Stop Move==========================================
    protected virtual void StoppingMove()
    {
        float xVel = this.user1.Value.GetRb(this).velocity.x;

        if (!this.user1.Value.CanStopMove(this)) return;
        if (xVel == 0) return;
        if (this.moveDir > 0 && xVel > Mathf.Pow(0.1f, 3)) return;
        if (this.moveDir < 0 && xVel < -Mathf.Pow(0.1f, 3)) return;
        this.StopMove();
    }

    protected virtual void StopMove()
    {
        Rigidbody2D rb = this.user1.Value.GetRb(this);
        float xVel = rb.velocity.x;
        float xForce = this.moveSpeed / moveSlowDownTime * Time.deltaTime;
        
        if (xVel > 0)
        {
            if (xForce > xVel) xForce = -xVel;
            else xForce = -xForce;
        }
        else if (xVel < 0)
        {
            if (xForce > -xVel) xForce = -xVel;
        }

        Vector2 force = new Vector2(xForce, 0);
        rb.AddForce(force, ForceMode2D.Impulse);
    }



    //============================================================================================
    //============================================Jump============================================
    //============================================================================================

    //===========================================Check============================================
    protected virtual void CheckingIsJumping()
    {
        if (this.isGround) this.isJumping = false;
        else if (this.jumpCD.IsReady) this.isJumping = false;
        else if (InputManager.Instance.SpaceState == 0) this.isJumping = false;
        else this.isJumping = true;
    }

    //============================================Jump============================================
    protected virtual void Jumping()
    {
        if (!this.user1.Value.CanJump(this)) return;
        if (!this.isGround) return;
        if (InputManager.Instance.SpaceState == 0) return;
        this.Jump();
    }

    protected virtual void Jump()
    {
        if (!this.jumpCD.IsReady) this.isJumping = true;
        else this.isJumping = false;

        if (this.isJumping == false) return;
        this.isGround = false;
        Vector2 force = new Vector2(0, this.GetYForce());
        this.user1.Value.GetRb(this).AddForce(force, ForceMode2D.Impulse);
    }

    protected virtual float GetYForce()
    {
        Rigidbody2D rb = this.user1.Value.GetRb(this);
        float yForce = jumpSpeed;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        return yForce;
    }

    //============================================Stop============================================
    protected virtual void StoppingJump()
    {
        Rigidbody2D rb = this.user1.Value.GetRb(this);

        if (rb.velocity.y <= 0) return;
        if (this.isGround) return;
        if (this.isJumping) return;
        this.StopJump();
    }

    protected virtual void StopJump()
    {
        Rigidbody2D rb = this.user1.Value.GetRb(this);
        float yForce = this.jumpSpeed / jumpSlowDownTime * Time.deltaTime;

        if (yForce > rb.velocity.y) yForce = rb.velocity.y;
        Vector2 force = new Vector2(0, -yForce);
        rb.AddForce(force, ForceMode2D.Impulse);
        Debug.Log("StopJump");
    }

    //==========================================Recharge==========================================
    protected virtual void RechargingJump()
    {
        if (!this.isJumping) return;
        this.RechargeJump();
    }

    protected virtual void RechargeJump()
    {
        this.jumpCD.CoolingDown();
    }

    //===========================================Finish===========================================
    protected virtual void FinishingJump()
    {
        if (!this.isGround) return;
        this.FinishJump();
    }

    protected virtual void FinishJump()
    {
        this.jumpCD.ResetStatus();
    }
}
