using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Bullet : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Bullet===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;

    [Space(25)]

    [Header("Stat")]
    [SerializeField] protected int damage;
    [SerializeField] protected float pushForce;
    [SerializeField] protected List<string> damageTags = new List<string>();

    [Space(25)]

    [Header("Despawn By Time")]
    [SerializeField] protected Cooldown despawnCD;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in this.damageTags)
        {
            if (!collision.CompareTag(tag)) continue;
            this.CollideWithTarget(collision);
            break;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.rb.velocity = Vector2.zero;
        this.despawnCD.ResetStatus();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.despawnCD.ResetStatus();
    }

    protected virtual void Update()
    {
        this.Despawning();
    }

    //======================================Despawn By Time=======================================
    protected virtual void Despawning()
    {
        Util.Instance.DespawnByTime(this.despawnCD, transform, BulletSpawner.Instance);
    }

    protected virtual void CollideWithTarget(Collider2D collision)
    {
        // Deal Damage
        Damagable damagable = collision.GetComponent<Damagable>();
        if (damagable == null) return;
        damagable.TakeDamage(this.damage);

        // Push
        float xDistance = collision.transform.position.x - transform.position.x;
        Vector2 dir = new Vector2(xDistance, 0).normalized;
        damagable.Push(this.pushForce * dir);
    }
}
