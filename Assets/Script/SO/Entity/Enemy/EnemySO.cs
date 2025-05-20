using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySO : EntitySO
{
    [Space(25)]
    [Header("===Enemy===")]
    [Header("Body Collide")]
    public int BodyDamage;
    public float BodyPushForce;
    public List<string> BodyAttackableTags;

    [Header("Other")]
    [SerializeField] public float GravityScale;
}
