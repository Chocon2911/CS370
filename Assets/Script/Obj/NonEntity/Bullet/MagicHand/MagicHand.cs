using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHand : HuyMonoBehaviour
{
    public enum AttackState
    {
        CHARGE = 0,
        ATTACK = 1,
    }

    //==========================================Variable==========================================
    [Header("===Magic Hand===")]
    [Header("Component")]
    [SerializeField] protected MagicHandAnimator animator;

    [Space(25)]

    [Header("Despawn By Time")]
    [SerializeField] protected Cooldown despawnCD;

    [Space(25)]

    [Header("Attack")]
    [SerializeField] protected CapsuleCollider2D attackCol;
    [SerializeField] protected int damage;
    [SerializeField] protected float pushForce;
    [SerializeField] protected LayerMask attackableLayer;
    [SerializeField] protected List<string> attackableTags = new List<string>();
    [SerializeField] protected AttackState currAttackState;
    [SerializeField] protected Cooldown chargeCD;
    [SerializeField] protected Cooldown attackCD;

    //==========================================Get Set===========================================
    public AttackState CurrAttackState => currAttackState;
    public Cooldown ChargeCD => chargeCD;
    public Cooldown AttackCD => attackCD;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform.Find("Model"), "LoadAnimator()");
        this.LoadComponent(ref this.attackCol, transform.Find("Model").Find("Attack"), "LoadAttackCol()");
    }

    protected virtual void Update()
    {
        this.Despawning();
        this.HanndlingAttack();
        this.animator.HandlingAnimator();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.currAttackState = AttackState.CHARGE;
        this.chargeCD.ResetStatus();
        this.attackCD.ResetStatus();
        this.despawnCD.ResetStatus();
    }

    //======================================Despawn By Time=======================================
    protected virtual void Despawning()
    {
        Util.Instance.DespawnByTime(this.despawnCD, transform, BulletSpawner.Instance);
    }

    //===========================================Attack===========================================
    protected virtual void HanndlingAttack()
    {
        this.Attacking();
        this.Charging();
    }

    protected virtual void Charging()
    {
        if (this.currAttackState != AttackState.CHARGE || this.chargeCD.IsReady) return;
        this.chargeCD.CoolingDown();

        if (!this.chargeCD.IsReady) return;
        this.currAttackState = AttackState.ATTACK;
    }

    protected virtual void Attacking()
    {
        if (this.currAttackState != AttackState.ATTACK) return;
        this.attackCD.CoolingDown();
        this.AttackColliding();

        if (!this.attackCD.IsReady) return;
        this.currAttackState = AttackState.CHARGE;
    }

    protected virtual void AttackColliding()
    {
        Vector2 pos = (Vector2)this.attackCol.transform.position + this.attackCol.offset;
        Vector2 size = this.attackCol.size;
        CapsuleDirection2D dir = this.attackCol.direction;

        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(pos, size, dir, 0, this.attackableLayer);
        foreach (Collider2D collider in colliders)
        {
            if (this.attackableTags.Contains(collider.tag))
            {
                Damagable damagable = collider.GetComponent<Damagable>();
                
                if (damagable == null) continue;
                damagable.TakeDamage(this.damage, transform);
                Vector2 pushDir = (collider.transform.position - this.transform.position).normalized;
                damagable.Push(pushDir * this.pushForce);
            }
        }
    }
}
