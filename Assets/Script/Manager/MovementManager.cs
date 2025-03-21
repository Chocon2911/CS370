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
        if (instance != null)
        {
            Debug.LogError("instance not null (transform)", transform.gameObject);
            Debug.LogError("Instance not null (instance)", instance.gameObject);
            return;
        }

        instance = this;
        base.Awake();
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

    public virtual void ChaseTarget(Transform user, Transform target, float speed) 
    {
        float xVel = target.position.x - user.position.x ;
        float yVel = target.position.y - user.position.y;
        user.Translate(new Vector3(xVel, yVel, 0) * speed * Time.deltaTime);
    }

    //============================================Jump============================================
    public void Jump(Rigidbody2D rb, float jumpSpeed)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    //============================================Stop============================================
    public void StopMove(Rigidbody2D rb)
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void StopJump(Rigidbody2D rb)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
    }

    public void Stop(Rigidbody2D rb)
    {
        rb.velocity = Vector2.zero;
    }
}
