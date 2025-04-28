using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet
{
    //==========================================Variable==========================================
    [Header("===Arrow===")]
    [Header("Move")]
    [SerializeField] protected float flySpeed;
    [SerializeField] protected bool canMove;

    //===========================================Unity============================================
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.canMove) return;
        base.OnTriggerEnter2D(collision);
    }

    protected override void Update()
    {
        base.Update();
        this.Moving();
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
        if (!this.canMove) return;
        Util.Instance.MoveForward(this.rb, this.flySpeed);
    }

    protected override void CollideWithTarget(Collider2D collision)
    {
        base.CollideWithTarget(collision);
        transform.parent = collision.transform;
    }
}
