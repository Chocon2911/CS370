using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : DbObj
{
    [Header("===Entity===")]
    [Header("Stat")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;

    [Space(25)]

    [Header("Hurt")]
    [SerializeField] protected Cooldown hurtCD;
    [SerializeField] protected bool isHurting;

    [Space(25)]

    [Header("Despawn By Time")]
    [SerializeField] protected Cooldown despawnCD;    

    //==========================================Get Set===========================================
    // Stat
    public int MaxHealth => maxHealth;
    public int Health => health;

    // Hurt
    public Cooldown HurtCD => this.hurtCD;
    public bool IsHurting => this.isHurting;

    //===========================================Unity============================================
    protected virtual void Update()
    {
        this.Hurting();
    }

    //===========================================Method===========================================
    protected virtual void DefaultEntity(EntitySO so)
    {
        this.maxHealth = so.maxHealth;
        this.health = this.maxHealth;
        this.hurtCD = new Cooldown(so.HurtDelay, 0);
    }

    //============================================Hurt============================================
    protected virtual void Hurting()
    {
        if (!this.isHurting) return;
        this.hurtCD.CoolingDown();

        if (!this.hurtCD.IsReady) return;
        this.hurtCD.ResetStatus();
        this.isHurting = false;
    }

    //==========================================Despawn===========================================
    protected abstract void Despawning();
}
