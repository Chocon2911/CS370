using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Entity/Archer")]
public class ArcherSO : GroundMonsterSO
{
    [Space(25)]
    [Header("===Archer===")]

    [Header("Chase Target")]
    public float StopChaseDistance;
    public float ChaseSpeed;

    [Space(10)]

    [Header("Bow Attack")]
    public float AttackDistance;
    public float BowRestoreDelay;
    public float ChargeBowDelay;
}
