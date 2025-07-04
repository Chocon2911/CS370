using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SpikeUser
{
    Transform GetTarget();
    Damagable GetDamagable();
}

[RequireComponent(typeof(CapsuleCollider2D))]
public class Spike : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Spike===")]
    [Header("Component")]
    [SerializeField] protected CapsuleCollider2D col;

    [Header("Stat")]
    [SerializeField] protected int damage;
    [SerializeField] protected int pushForce;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.col, transform, "LoadCol()");
    }

    //===========================================Method===========================================
    public void Collide(SpikeUser user)
    {
        float xDir = user.GetTarget().position.x - transform.position.x;
        float yDir = user.GetTarget().position.y - transform.position.y;
        Vector2 pushDir = new Vector2(xDir, yDir).normalized;

        user.GetDamagable().TakeDamage(this.damage, user.GetTarget());
        user.GetDamagable().Push(this.pushForce * pushDir);
    }
}
