using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public abstract class Bullet : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Bullet===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CircleCollider2D col;

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
        this.LoadComponent(ref this.col, transform, "LoadCol()");
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
        damagable.TakeDamage(this.damage);

        // Push
        float xDistance = collision.transform.position.x - transform.position.x;
<<<<<<< HEAD
        Vector2 dir = new Vector2(xDistance, 0).normalized;
=======
        float yDistance = collision.transform.position.y - transform.position.y;
        Vector2 dir = new Vector2(xDistance, yDistance).normalized;
>>>>>>> 2f2a3976610e0664b609c5e885ef2642e65f8eb7
        damagable.Push(this.pushForce * dir);

        // Stick to Collision
        transform.parent = collision.transform;
    }
}
