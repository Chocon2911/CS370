using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KatanaState
{
    ATTACK = 1,
    FINISH = 2,
    IDLE = 3,
}

public class Katana : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("Component")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected CircleCollider2D damageCol;
    [SerializeField] protected TrailRenderer slashTrail;
    [SerializeField] protected Transform katanaObj;
    [SerializeField] protected KatanaSO so;
    [SerializeField] protected Transform wielder;

    [Header("Damage Collision")]
    [SerializeField] protected List<Transform> attackedObj = new List<Transform>();
    [SerializeField] protected List<string> attackableTags = new List<string>();
    [SerializeField] protected LayerMask attackableLayer;
    [SerializeField] protected int damage;
    [SerializeField] protected float pushForce;

    [Header("Attack")]
    [SerializeField] protected Cooldown restoreCD;
    [SerializeField] protected Cooldown attackCD;
    [SerializeField] protected bool isAttacking;

    //==========================================Get Set===========================================
    public bool IsAttacking => this.isAttacking;

    public Transform Wielder
    {
        get => this.wielder;
        set => this.wielder = value;
    }

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.animator, transform, "LoadAnimator()");
        this.LoadComponent(ref this.damageCol, transform.Find("Obj"), "LoadDamageCol()");
        this.LoadComponent(ref this.slashTrail, transform.Find("Obj").Find("Trail"), "LoadSlashTrail()");
        this.LoadComponent(ref this.katanaObj, transform.Find("Obj"), "LoadKatanaObj()");
        this.LoadComponent(ref this.wielder, transform.parent, "LoadWielder()");
        this.DefaultStat();
    }

    public override void MyUpdate()
    {
        //this.HandlingTrailPos();
        this.Restoring();
        this.CheckingCol();
        this.Attacking();
    }

    //===========================================Trail============================================
    private void HandlingTrailPos()
    {
        Vector3 localPosition = this.slashTrail.transform.InverseTransformPoint(transform.position);
        this.slashTrail.transform.localPosition = localPosition;
    }

    //=========================================Damage Col=========================================
    private void CheckingCol()
    {
        if (!this.isAttacking) return;
        Vector2 pos = (Vector2)this.katanaObj.position + (this.damageCol.offset * Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad));
        float radius = this.damageCol.radius;
        Collider2D[] collisions = Physics2D.OverlapCircleAll(pos, radius, this.attackableLayer);

        foreach (Collider2D collision in collisions)
        {
            if (this.attackedObj.Contains(collision.transform)) continue;
            this.DealDamage(collision.transform);
            this.SplashEffect(collision.transform);
        }
    }

<<<<<<< HEAD
            if (damagable != null)
            {
                foreach (string tag in this.attackableTags)
                {
                    if (!collision.CompareTag(tag)) continue; 
                    damagable.TakeDamage(this.damage);
                    Vector2 pushDir = (collision.transform.position - this.transform.position).normalized;
                    damagable.Push(pushDir * this.pushForce);
                    this.attackedObj.Add(collision.transform);
                    break;
                }
            }

            EffectSplashable splashable = collision.GetComponent<EffectSplashable>();

            if (splashable != null)
            {
                splashable.Splash(this.wielder.position);
            }
=======
    private void DealDamage(Transform colTrans)
    {
        Damagable damagable = colTrans.GetComponent<Damagable>();

        if (damagable == null) return;
        foreach (string tag in this.attackableTags)
        {
            if (!colTrans.CompareTag(tag)) continue;
            damagable.TakeDamage(this.damage);
            Vector2 pushDir = (colTrans.position - this.transform.position).normalized;
            damagable.Push(pushDir * this.pushForce);
            this.attackedObj.Add(colTrans);
            break;
        }
    }

    private void SplashEffect(Transform colTrans)
    {
        EffectSplashable splashable = colTrans.GetComponent<EffectSplashable>();

        if (splashable == null) return;
        foreach (string tag in this.attackableTags)
        {
            if (!colTrans.CompareTag(tag)) continue;
            splashable.Splash(this.wielder.position);
            break;
>>>>>>> 2f2a3976610e0664b609c5e885ef2642e65f8eb7
        }
    }

    //===========================================Attack===========================================
    public void Attack()
    {
        if (this.isAttacking || !this.restoreCD.IsReady) return;
        this.isAttacking = true;
        this.katanaObj.gameObject.SetActive(true);
        this.restoreCD.ResetStatus();
        this.animator.SetFloat("Speed", 1 / this.attackCD.TimeLimit);
        this.animator.SetInteger("State", (int)KatanaState.ATTACK);
    }

    public void FinishAttack()
    {
        this.attackCD.ResetStatus();
        this.animator.SetInteger("State", (int)KatanaState.FINISH);
        this.isAttacking = false;
        this.katanaObj.gameObject.SetActive(false);
        this.attackedObj.Clear();
    }

    private void Restoring()
    {
        if (this.isAttacking || this.restoreCD.IsReady) return;
        this.restoreCD.CoolingDown();
    }

    private void Attacking()
    {
        if (!this.isAttacking) return;
        this.attackCD.CoolingDown();

        if (!this.attackCD.IsReady) return;
        this.FinishAttack();
    }

    //===========================================Other============================================
    public void DefaultStat()
    {
        if (this.so == null)
        {
            Debug.LogError("No KatanaSO", transform.gameObject);
            return;
        }

        // Damage Collision
        this.damage = this.so.damage;
        this.pushForce = this.so.pushForce;

        // Attack
        this.attackableTags = this.so.attackableTags;
        this.attackableLayer = this.so.attackableLayer;
        this.attackCD = new Cooldown(this.so.attackDelay, 0);
        this.restoreCD = new Cooldown(this.so.restoreDelay, 0);
    }
}
