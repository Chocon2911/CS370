using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : HuyMonoBehaviour
{
    [Header("===Entity===")]
    [Header("Basic")]
    [SerializeField] protected string id;

    [Space(25)]

    [Header("Stat")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;

    [Space(25)]

    [Header("Hurt")]
    [SerializeField] protected Cooldown hurtCD;
    [SerializeField] protected bool isHurting;

    //==========================================Get Set===========================================
    // Stat
    public string Id => id;
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
    public void RandomId()
    {
        this.id = System.Guid.NewGuid().ToString();
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
}
