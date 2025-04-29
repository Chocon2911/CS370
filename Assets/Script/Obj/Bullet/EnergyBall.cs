using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class EnergyBall : Bullet
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Energy Ball===")]
    [Header("Move")]
    [SerializeField] protected float flySpeed;

    //===========================================Unity============================================
    protected override void Update()
    {
        base.Update();
        this.Moving();
    }

    //============================================Move============================================
    protected virtual void Moving()
    {
        Util.Instance.MoveForward(this.rb, this.flySpeed);
    }
}
