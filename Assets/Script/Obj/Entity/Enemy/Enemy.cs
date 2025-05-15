using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class Enemy : Entity, Damagable, EffectSplashable
{
    //==========================================Variable==========================================
    [Space(50)]
    [Header("===Enemy===")]
    [Header("Component")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D bodyCol;
    [SerializeField] protected ParticleSystem damageEff;

    [Space(25)]

    [Header("Stat")]
    [SerializeField] protected int sceneIndex;
    [SerializeField] protected MonsterType monsterType;

    [Space(25)]

    [Header("Body Collide")]
    [SerializeField] protected List<string> bodyAttackableTags;
    [SerializeField] protected int bodyDamage;
    [SerializeField] protected float bodyPushForce;

    //==========================================Get Set===========================================
    // ===Db===
    public MonsterDbData Db
    {
        get
        {
            return new MonsterDbData(this.sceneIndex, this.monsterType, this.health, this.maxHealth, this.id);
        }

        set
        {
            this.sceneIndex = value.SceneIndex;
            this.monsterType = value.Type;
            this.health = value.Health;
            this.id = value.Id;
        }
    }

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.LoadComponent(ref this.damageEff, transform.Find("DamageEffect"), "LoadDamageEff()");
    }

    protected override void Awake()
    {
        base.Awake();
        //this.LoadDb();
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (this.bodyAttackableTags.Contains(collision.gameObject.tag))
        {
            Damagable damagable = collision.gameObject.GetComponent<Damagable>();
            if (damagable == null) return;
            damagable.TakeDamage(this.bodyDamage);
            Vector2 dir = (collision.transform.position - this.transform.position).normalized;
            damagable.Push(this.bodyPushForce * dir);
        }
    }



    //============================================================================================
    //===========================================Method===========================================
    //============================================================================================

    //==========================================Database==========================================
    protected virtual void LoadDb()
    {
        if (DataBaseManager.Instance.Monster.IsMonsterExist(this.id))
        {
            this.Db = DataBaseManager.Instance.Monster.Query(this.id);
        }

        else
        {
            DataBaseManager.Instance.Monster.Insert(this.Db);
        }
    }

    protected virtual void OnSceneEnd()
    {
        DataBaseManager.Instance.Monster.Update(this.Db);
    }

    //=======================================Damage Effect========================================
    protected virtual void PlayDamageEffect(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.damageEff.transform.rotation = Quaternion.Euler(0, 0, angle - 2f);
        this.damageEff.Play();
    }



    //============================================================================================
    //=========================================Interface==========================================
    //============================================================================================

    //=========================================Damagable==========================================
    void Damagable.TakeDamage(int damage)
    {
        this.health -= damage;
        this.isHurting = true;

        if (this.health <= 0)
        {
            this.isHurting = false;
            this.health = 0;
            Debug.Log("Dead", gameObject);
        }
    }

    void Damagable.Push(Vector2 force)
    {
        this.rb.velocity = force;
    }

    //=====================================Effect Splashable======================================
    void EffectSplashable.Splash(Vector2 pos)
    {
        Vector2 dir = ((Vector2)transform.position - pos).normalized;
        this.PlayDamageEffect(dir);
    }
}
