using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterSO : EnemySO
{
    [Space(25)]
    [Header("===Monster===")]
    [Header("Target Detection")]
    public LayerMask TargetLayer;
    public string TargetTag;

    [Space(10)]

    [Header("Move")]
    public float SlowDownTime;
    public float SpeedUpTime;

    [Space(10)]

    [Header("Move Randomly")]
    public float SlowSpeed;
}
