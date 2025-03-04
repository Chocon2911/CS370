using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] protected float jumpSpeed;

    //===========================================Method===========================================
    public virtual void DoJump(Rigidbody2D rb)
    {
        float yVel = this.jumpSpeed;
        float xVel = rb.velocity.x;

        rb.velocity = new Vector2(xVel, yVel);
    }

    public virtual void StopJump(Rigidbody2D rb)
    {
        float xVel = rb.velocity.x;
        rb.velocity = new Vector2(xVel, 0);
    }
}
