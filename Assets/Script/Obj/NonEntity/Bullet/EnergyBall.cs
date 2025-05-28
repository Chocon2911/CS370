using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class EnergyBall : HuyMonoBehaviour
{
    [Header("===Energy Ball===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CircleCollider2D col;

    [Header("Stat")]
    [SerializeField] protected int damage;
    [SerializeField] protected List<string> damageTags = new List<string>();

    [Header("Move")]
    [SerializeField] protected float flySpeed;

    [Header("Despawn By Time")]
    [SerializeField] protected Cooldown despawnCD;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.col, transform, "LoadCol()");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in this.damageTags)
        {
            if (!collision.CompareTag(tag)) continue;
            collision.GetComponent<Damagable>().TakeDamage(this.damage, transform);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.rb.velocity = Vector2.zero;
        this.despawnCD.ResetStatus();
    }

    protected virtual void Update()
    {
        this.Moving();
        this.Despawning();
    }

    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //============================================Move============================================
    protected virtual void Moving()
    {
        Util.Instance.MoveForward(this.rb, this.flySpeed);
    }

    //======================================Despawn By Time=======================================
    protected virtual void Despawning()
    {
        Util.Instance.DespawnByTime(this.despawnCD ,transform, BulletSpawner.Instance);
    }
}
