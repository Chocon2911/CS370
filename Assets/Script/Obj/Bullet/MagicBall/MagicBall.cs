using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class MagicBall : Bullet
{
    //==========================================Variable==========================================
    [Header("===Magic Ball===")]
    [Header("Component")]
    [SerializeField] protected CircleCollider2D bodyCol;

    [Header("Chase Target")]
    [SerializeField] protected Transform target;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotSpeed;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
    }

    protected override void Update()
    {
        base.Update();
        this.Moving();
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
        if (this.target == null) return;
        this.MoveForward();
        this.RotateToTarget();
    }

    protected virtual void MoveForward()
    {
        float angle = transform.rotation.eulerAngles.z;
        float xDir = Mathf.Cos(angle * Mathf.Deg2Rad);
        float yDir = Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector2 dir = new Vector2(xDir, yDir).normalized;
        this.rb.velocity = dir * this.moveSpeed;
    }

    protected virtual void RotateToTarget()
    {
        Vector3 dir = this.target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, this.rotSpeed * Time.deltaTime);
    }

    //==========================================Collide===========================================
    protected override void CollideWithTarget(Collider2D collision)
    {
        base.CollideWithTarget(collision);
        Util.Instance.Despawn(BulletSpawner.Instance, transform);
    }
}
