using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static MovementManager instance;
    public static MovementManager Instance => instance;

    //===========================================Unity============================================
    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("One Instance only", transform.gameObject);
            return;
        }

        instance = this;
    }

    //============================================Move============================================
    public void Move(Rigidbody2D rb, Vector2 vel)
    {
        rb.velocity += vel;
    }

    public void Move(Rigidbody2D rb, int dir, float speed, float speedUpTime, float slowDownTime)
    {
        float xVel = rb.velocity.x;
        float applySpeed = 0;

        if (dir > 0)
        {
            if (xVel > Mathf.Pow(-0.1f, 3))
            {
                applySpeed = dir * speed / speedUpTime * Time.deltaTime;
                if (applySpeed > speed - xVel) applySpeed = speed - xVel;
            }
            else
            {
                applySpeed = dir * speed / slowDownTime * Time.deltaTime;
                if (applySpeed > -xVel) applySpeed = -xVel;
            }
        }

        else if (dir < 0)
        {
            if (xVel < Mathf.Pow(0.1f, 3))
            {
                applySpeed = dir * speed / speedUpTime * Time.deltaTime;
                if (applySpeed < -speed - xVel) applySpeed = -speed - xVel;
            }
            else
            {
                applySpeed = dir * speed / slowDownTime * Time.deltaTime;
                if (-applySpeed > xVel) applySpeed = -xVel;
            }
        }

        else
        {
            if (xVel > Mathf.Pow(0.1f, 3))
            {
                applySpeed = -speed / slowDownTime * Time.deltaTime;
                if (-applySpeed > xVel) applySpeed = -xVel;
            }
            else if (xVel < Mathf.Pow(-0.1f, 3))
            {
                applySpeed = speed / slowDownTime * Time.deltaTime;
                if (applySpeed > -xVel) applySpeed = -xVel;
            }
        }
        
        this.Move(rb, new Vector2(applySpeed, 0));
    }

    public void StopMove(Rigidbody2D rb)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    //============================================Jump============================================
    public void Jump(Rigidbody2D rb, float jumpSpeed)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    public void StopJump(Rigidbody2D rb)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    //=========================================Stop Move==========================================
    public void Stop(Rigidbody2D rb)
    {
        rb.velocity = Vector2.zero;
    }
}
