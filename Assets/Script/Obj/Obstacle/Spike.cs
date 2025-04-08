using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public interface ISpike
{
    Transform GetTarget();
    void Damage(int damage);
    void Push(int pushForce, Vector2 pushDir);
}

[RequireComponent(typeof(BoxCollider2D))]
public class Spike : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Spike===")]
    [Header("Component")]
    [SerializeField] protected BoxCollider2D col;

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
    public void Collide(ISpike user)
    {
        float xDir = user.GetTarget().position.x - transform.position.x;
        float yDir = user.GetTarget().position.y - transform.position.y;
        Vector2 pushDir = new Vector2(xDir, yDir).normalized;

        user.Damage(this.damage);
        user.Push(this.pushForce, pushDir);
    }
}
