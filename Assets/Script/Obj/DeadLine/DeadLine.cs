using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class DeadLine : HuyMonoBehaviour
{
    [Header("===Dead Line===")]
    [SerializeField] protected CapsuleCollider2D bodyCol;
    private const int damage = 999999;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damagable damagable = collision.GetComponent<Damagable>();
        
        if (damagable == null) return;
        damagable.TakeDamage(damage, transform);
    }
}
