using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : HuyMonoBehaviour
{
    [Header("===Entity===")]
    [Header("Basic")]
    [SerializeField] protected string id;

    [Header("Stat")]
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;

    //==========================================Get Set===========================================
    public string Id => id;
    public int MaxHealth => maxHealth;
    public int Health => health;

    public void RandomId()
    {
        this.id = System.Guid.NewGuid().ToString();
    }
}
