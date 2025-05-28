using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HealthPotion : HuyMonoBehaviour
{
    [Header("===Health Potion===")]
    [SerializeField] protected int restoreValue;
    public int RestoreValue => restoreValue;
    
    public void PickedUp()
    {
        ItemSpawner.Instance.Despawn(transform);
    }
}
