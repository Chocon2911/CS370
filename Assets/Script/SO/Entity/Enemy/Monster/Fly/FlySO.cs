using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Fly", order = 1)]
public class FlySO : MonsterSO
{
    [Space(25)]
    [Header("===Fly===")]
    [Header("Run Away")]
    public float RunAwaySpeed;
    public float StopRunAwayDistance;

    [Space(10)]

    [Header("Fire")]
    public string BulletName;
    public float AttackDistance;
    public float FireRestoreDelay;
    public float ChargeFireDelay;
}
