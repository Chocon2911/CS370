using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Arrow : Bullet
{
    //==========================================Variable==========================================
    [Header("===Arrow===")]
    [Header("Component")]
    [SerializeField] protected CircleCollider2D col;

    [Header("Move")]
    [SerializeField] protected float flySpeed;
    [SerializeField] protected bool isCollided;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.col, transform, "LoadCol()");
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.isCollided) return;
        base.OnTriggerEnter2D(collision);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.isCollided = false;
    }

    protected override void Update()
    {
        base.Update();
        this.Moving();
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
        if (this.isCollided)
        {
            this.rb.velocity = Vector2.zero;
            return;
        }

        Util.Instance.MoveForward(this.rb, this.flySpeed);
    }

    protected override void CollideWithTarget(Collider2D collision)
    {
        base.CollideWithTarget(collision);
        // Stick to Collision
        transform.parent = collision.transform;
        this.isCollided = true;
    }
}
