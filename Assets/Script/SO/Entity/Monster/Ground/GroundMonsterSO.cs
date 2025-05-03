using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundMonsterSO : MonsterSO
{
    [Space(25)]
    [Header("===Ground Monster===")]
    [Header("Ground Check")]
    public LayerMask GroundLayer;
    public string GroundTag;

    [Space(10)]

    [Header("Wall Detection")]
    public float WallDetectDistance;
    public LayerMask WallLayer;
    public string WallTag;

    [Space(10)]

    [Header("Target Detection")]
    public float TargetDetectDistance;

    [Space(10)]

    [Header("Move")]
    public float SlowDownTime;
    public float SpeedUpTime;

    [Space(10)]

    [Header("Jump")]
    public float JumpSpeed;
}
