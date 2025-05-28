using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class SlashWave : Bullet
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Slash Wave===")]
    [Header("Component")]
    [SerializeField] protected CapsuleCollider2D bodyCol;

    [Header("Move Forward")]
    [SerializeField] protected float flySpeed;

    [Header("Grow Up")]
    [SerializeField] protected Cooldown growUpCD;
    [SerializeField] protected Vector3 startScale;
    [SerializeField] protected Vector3 goalScale;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.startScale = transform.localScale;
    }

    protected override void Update()
    {
        base.Update();
        this.GrowingUp();
        this.Moving();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.growUpCD.ResetStatus();
        transform.localScale = this.startScale;
    }

    //===========================================Method===========================================
    protected virtual void Moving()
    {
        float angle = transform.rotation.eulerAngles.z;
        float xDir = Mathf.Cos(angle * Mathf.Deg2Rad);
        float yDir = Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector2 dir = new Vector2(xDir, yDir).normalized;
        this.rb.velocity = dir * this.flySpeed;
    }
    
    protected virtual void GrowingUp()
    {
        if (this.growUpCD.IsReady) return;
        this.growUpCD.CoolingDown();
        transform.localScale = this.startScale + (this.goalScale - this.startScale) * (this.growUpCD.Timer / this.growUpCD.TimeLimit);
    }
}
